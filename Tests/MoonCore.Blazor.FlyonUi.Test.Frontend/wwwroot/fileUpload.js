window.moonCoreFileUpload = {
    init: function (dropZoneId, fileInputId, callbackRef) {

        const dropZone = document.getElementById(dropZoneId);
        const fileInput = document.getElementById(fileInputId);

        const doUpload = async function (path, file) {
            //const stream = file.stream();

            if (file.size === 0)
                return;

            const streamRef = DotNet.createJSStreamReference(file);

            await callbackRef.invokeMethodAsync("Handle", path, streamRef);
        };

        const handleDragEvent = async function (dragEvent) {

            // Prevent default shit
            dragEvent.preventDefault();
            dragEvent.stopPropagation();

            const items = dragEvent.dataTransfer?.items || [];

            // First extract all webkit entries while DataTransfer is still accessible
            // cause when we do our async interop with blazor it will be gone.
            // Don't ask why...

            const entries = [];
            const files = [];

            for (let i = 0; i < items.length; i++) {
                const item = items[i];

                if (item.kind !== 'file')
                    continue;

                const entry = item.webkitGetAsEntry?.();

                if (entry) {
                    entries.push(entry);
                } else {
                    // Fallback for browsers without webkitGetAsEntry support.
                    // I dunno who would use such an outdated browser but whatever
                    const file = item.getAsFile();

                    if (!file) {
                        console.log("Unable to get file or entry from item", item);
                        continue;
                    }

                    files.push(file);
                }
            }

            // Second process all collected entries
            for (const entry of entries) {
                await callbackRef.invokeMethodAsync("Start", entry.name);

                await processEntry(entry);

                await callbackRef.invokeMethodAsync("Stop");
            }

            // Third process fallback files
            for (const file of files) {
                await doUpload(file.name, file);
            }
        };

        const processEntry = async function (entry, path = '') {
            if (entry.isFile) {
                const file = await new Promise((resolve, reject) => {
                    entry.file(resolve, reject);
                });

                const fullPath = path ? `${path}/${file.name}` : file.name;
                await doUpload(fullPath, file);
            } else if (entry.isDirectory) {
                const reader = entry.createReader();
                const entries = await new Promise((resolve, reject) => {
                    reader.readEntries(resolve, reject);
                });

                for (const childEntry of entries) {
                    const childPath = path ? `${path}/${entry.name}` : entry.name;
                    await processEntry(childEntry, childPath);
                }
            }
        }

        // Prevent default behavior for drag events
        const preventDefaults = (e) => {
            e.preventDefault();
            e.stopPropagation();
        };

        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach((eventName) => {
            dropZone.addEventListener(eventName, preventDefaults, false);
        });

        dropZone.addEventListener('drop', async (e) => {
            const items = e.dataTransfer.items;

            if (!items) return;

            await handleDragEvent(e);
        });

        fileInput.addEventListener("change", async function (ev) {
            await callbackRef.invokeMethodAsync("Start", ev.target.files.length > 1 ? "multiple files" : "file");

            for (const file of ev.target.files) {
                await doUpload(file.name, file);
            }

            await callbackRef.invokeMethodAsync("Stop");
        });
    }
}
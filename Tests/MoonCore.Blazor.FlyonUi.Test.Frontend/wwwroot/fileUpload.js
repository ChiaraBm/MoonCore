window.moonCoreFileUpload = {
    init: function () {

    },
    registerDropHandler: function (elementId, inputId, callbackRef) {
        const dropZone = document.getElementById(elementId);
        const input = document.getElementById(inputId);
        if (!dropZone) {
            console.error(`Element with id "${elementId}" not found.`);
            return;
        }

        const doUpload = async function (path, file) {
            //const stream = file.stream();

            if (file.size === 0)
                return;

            const streamRef = DotNet.createJSStreamReference(file);
            
            await callbackRef.invokeMethodAsync("Handle", path, streamRef);
        };

        // Prevent default behavior for drag events
        const preventDefaults = (e) => {
            e.preventDefault();
            e.stopPropagation();
        };

        ['dragenter', 'dragover', 'dragleave', 'drop'].forEach((eventName) => {
            dropZone.addEventListener(eventName, preventDefaults, false);
        });

        dropZone.addEventListener('drop', async (e) => {
            try {
                const items = e.dataTransfer.items;
                if (!items) return;

                await this.handleDragEvent(e, doUpload, callbackRef);
            }
            catch (e) {
                console.log(e);
            }
        });

        input.addEventListener("change", async function (ev) {
            await callbackRef.invokeMethodAsync("Start", ev.target.files.length > 1 ? "multiple files" : "file");

            for (const file of ev.target.files) {
                await doUpload(file.name, file);
            }

            await callbackRef.invokeMethodAsync("Stop");
        })
    },

    handleDragEvent: async function (dragEvent, callback, callbackRef) {
        dragEvent.preventDefault();
        dragEvent.stopPropagation();

        const items = dragEvent.dataTransfer?.items || [];

        // First pass: Extract all webkit entries while DataTransfer is still accessible
        const entries = [];
        const files = [];

        for (let i = 0; i < items.length; i++) {
            const item = items[i];

            if (item.kind === 'file') {
                const entry = item.webkitGetAsEntry?.();

                if (entry) {
                    entries.push(entry);
                } else {
                    // Fallback for browsers without webkitGetAsEntry support
                    const file = item.getAsFile();
                    if (file) {
                        files.push(file);
                    } else {
                        console.log("Unable to get file or entry from item", item);
                    }
                }
            }
        }

        // Second pass: Process all collected entries
        for (const entry of entries) {
            console.log("Processing entry:", entry);

            await callbackRef.invokeMethodAsync("Start", entry.name);
            console.log("Uploading directory: " + entry.name);
            await this.processEntry(entry, callback);

            await callbackRef.invokeMethodAsync("Stop");
        }

        // Third pass: Process fallback files
        for (const file of files) {
            console.log("Processing file:", file.name);
            await callback(file.name, file);
        }
    },

    processEntry: async function (entry, callback, path = '') {
        if (entry.isFile) {
            const file = await new Promise((resolve, reject) => {
                entry.file(resolve, reject);
            });

            const fullPath = path ? `${path}/${file.name}` : file.name;
            await callback(fullPath, file);
        } else if (entry.isDirectory) {
            const reader = entry.createReader();
            const entries = await new Promise((resolve, reject) => {
                reader.readEntries(resolve, reject);
            });

            for (const childEntry of entries) {
                const childPath = path ? `${path}/${entry.name}` : entry.name;
                await this.processEntry(childEntry, callback, childPath);
            }
        }
    }
}
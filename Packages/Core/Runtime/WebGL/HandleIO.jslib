var HandleIO = {
    WindowAlert: function (message) {
        window.alert(Pointer_stringify(message));
    },
    SyncFiles: function () {
        FS.syncfs(false, function (err) {

        });
    },
    DownloadFile: function (array, size, fileNamePtr) {
        var fileName = UTF8ToString(fileNamePtr);

        var bytes = new Uint8Array(size);
        for (var i = 0; i < size; i++) {
            bytes[i] = HEAPU8[array + i];
        }

        var blob = new Blob([bytes]);
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = fileName;

        var event = document.createEvent("MouseEvents");
        event.initMouseEvent("click");
        link.dispatchEvent(event);
        window.URL.revokeObjectURL(link.href);
        //link.parentNode.removeChild(link); // This throws an exception, but I guess it's important somehow.
    },
    GetFileFromBrowser: function (objectNamePtr, funcNamePtr, filesToAcceptPtr, chooseFileTextPtr, cancelTextPtr, noFileSelectedTextPtr, cancelledTextPtr) {
        // Because unity is currently bad at JavaScript we can't use standard
        // JavaScript idioms like closures so we have to use global variables :(
        window.becauseUnitysBadWithJavacript_getFileFromBrowser =
            window.becauseUnitysBadWithJavacript_getFileFromBrowser || {
                busy: false,
                initialized: false,
                rootDisplayStyle: null,  // style to make root element visible
                root_: null,             // root element of form
                ctx_: null               // canvas for getting image data;
            };
        var g = window.becauseUnitysBadWithJavacript_getFileFromBrowser;
        if (g.busy) {
            // Don't let multiple requests come in
            return;
        }
        g.busy = true;

        var objectName = Pointer_stringify(objectNamePtr);
        var funcName = Pointer_stringify(funcNamePtr);
        var filesToAccept = Pointer_stringify(filesToAcceptPtr);
        var chooseFileText = Pointer_stringify(chooseFileTextPtr);
        var cancelText = Pointer_stringify(cancelTextPtr);
        var noFileSelectedText = Pointer_stringify(noFileSelectedTextPtr);
        var cancelledText = Pointer_stringify(cancelledTextPtr);

        if (!g.initialized) {
            g.initialized = true;
            g.ctx = window.document.createElement("canvas").getContext("2d");

            // Append a form to the page (more self contained than editing the HTML?)
            g.root = window.document.createElement("div");
            g.root.innerHTML = [
                '<style>                                                    ',
                '.getfile {                                                 ',
                '    position: absolute;                                    ',
                '    left: 0;                                               ',
                '    top: 0;                                                ',
                '    width: 100%;                                           ',
                '    height: 100%;                                          ',
                '    display: -webkit-flex;                                 ',
                '    display: flex;                                         ',
                '    -webkit-flex-flow: column;                             ',
                '    flex-flow: column;                                     ',
                '    -webkit-justify-content: center;                       ',
                '    -webkit-align-content: center;                         ',
                '    -webkit-align-items: center;                           ',
                '                                                           ',
                '    justify-content: center;                               ',
                '    align-content: center;                                 ',
                '    align-items: center;                                   ',
                '                                                           ',
                '    z-index: 2;                                            ',
                '    color: white;                                          ',
                '    background-color: rgba(0,0,0,0.8);                     ',
                '    font: sans-serif;                                      ',
                '    font-size: x-large;                                    ',
                '}                                                          ',
                '.getfile a,                                                ',
                '.getfile label {                                           ',
                '   font-size: x-large;                                     ',
                '   background-color: #666;                                 ',
                '   border-radius: 0.5em;                                   ',
                '   border: 1px solid black;                                ',
                '   padding: 0.5em;                                         ',
                '   margin: 0.25em;                                         ',
                '   outline: none;                                          ',
                '   display: inline-block;                                  ',
                '}                                                          ',
                '.getfile input {                                           ',
                '    display: none;                                         ',
                '}                                                          ',
                '</style>                                                   ',
                '<div class="getfile">                                      ',
                '    <div>                                                  ',
                '      <label for="file">' + chooseFileText + '</label>     ',
                '      <input id="file" type="file" accept="' + filesToAccept + '"/><br/>',
                '      <a>' + cancelText + '</a>                                        ',
                '    </div>                                                 ',
                '</div>                                                     '
            ].join('\n');
            var input = g.root.querySelector("input");
            input.addEventListener('change', getFile);

            // prevent clicking in input or label from canceling
            input.addEventListener('click', preventOtherClicks);
            var label = g.root.querySelector("label");
            label.addEventListener('click', preventOtherClicks);

            // clicking cancel or outside cancels
            var cancel = g.root.querySelector("a");  // there's only one
            cancel.addEventListener('click', handleCancel);
            var getImage = g.root.querySelector(".getfile");
            getImage.addEventListener('click', handleCancel);

            // remember the original style
            g.rootDisplayStyle = g.root.style.display;

            window.document.body.appendChild(g.root);
        }

        // make it visible
        g.root.style.display = g.rootDisplayStyle;

        function preventOtherClicks(evt) {
            evt.stopPropagation();
        }

        function getFile(evt) {
            evt.stopPropagation();
            var fileInput = evt.target.files;
            if (!fileInput || !fileInput.length) {
                return sendError(noFileSelectedText);
            }
            
            var url = window.URL.createObjectURL(fileInput[0]);

            sendResult(url);
            window.URL.revokeObjectURL(file.src);
            g.busy = false;            
        }

        function handleCancel(evt) {
            evt.stopPropagation();
            evt.preventDefault();
            sendError(cancelledText);
        }
        

        function sendError(msg) {
            sendResult(msg);
        }

        function hide() {
            g.root.style.display = "none";
        }

        function sendResult(result) {
            hide();
            g.busy = false;
            SendMessage(objectName, funcName, result);
        }
    }
};

mergeInto(LibraryManager.library, HandleIO);
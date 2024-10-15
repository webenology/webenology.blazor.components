// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

let link = document.createElement("link");
link.rel = "stylesheet";
link.href = "./_content/webenology.blazor.components.JoditEditor/jodit.min.css";
document.head.appendChild(link);
let scriptEle = document.createElement("script");
scriptEle.setAttribute("src", "./_content/webenology.blazor.components.JoditEditor/jodit.min.js");
document.head.appendChild(scriptEle);

export class JoditHelper {

    Setup(id, mergeTags, dotnet) {
        console.log("edoitor", this.editor);
        console.log(mergeTags);

        var interval = setInterval(() => {
            if (Jodit != null) {

                let buttons = ['undo', 'redo', '|',
                    'bold', 'italic', 'underline', '|',
                    'align', 'font', 'fontsize', '|',
                    'ul', 'ol', '|',
                    'table', 'link', 'image'];


                if (mergeTags) {
                    Jodit.defaultOptions.controls.mergeTags = {
                        icon: '<svg width="800px" height="800px" viewBox="0 0 16 16" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">' +
                            '<path fill="#444" d="M2.1 3.1c0.2 1.3 0.4 1.6 0.4 2.9 0 0.8-1.5 1.5-1.5 1.5v1c0 0 1.5 0.7 1.5 1.5 0 1.3-0.2 1.6-0.4 2.9-0.3 2.1 0.8 3.1 1.8 3.1s2.1 0 2.1 0v-2c0 0-1.8 0.2-1.8-1 0-0.9 0.2-0.9 0.4-2.9 0.1-0.9-0.5-1.6-1.1-2.1 0.6-0.5 1.2-1.1 1.1-2-0.3-2-0.4-2-0.4-2.9 0-1.2 1.8-1.1 1.8-1.1v-2c0 0-1 0-2.1 0s-2.1 1-1.8 3.1z"></path>' +
                            '<path fill="#444" d="M13.9 3.1c-0.2 1.3-0.4 1.6-0.4 2.9 0 0.8 1.5 1.5 1.5 1.5v1c0 0-1.5 0.7-1.5 1.5 0 1.3 0.2 1.6 0.4 2.9 0.3 2.1-0.8 3.1-1.8 3.1s-2.1 0-2.1 0v-2c0 0 1.8 0.2 1.8-1 0-0.9-0.2-0.9-0.4-2.9-0.1-0.9 0.5-1.6 1.1-2.1-0.6-0.5-1.2-1.1-1.1-2 0.2-2 0.4-2 0.4-2.9 0-1.2-1.8-1.1-1.8-1.1v-2c0 0 1 0 2.1 0s2.1 1 1.8 3.1z"></path>' +
                            '</svg>',
                        list: mergeTags,
                        childTemplate: (editor, key, value) =>
                            `<span class="${key}">${editor.i18n(value)}</span>`,

                        exec(editor, _, { control }) {
                            if (control.name == "mergeTags")
                                return false;
                            let key = control.args && control.args[0];
                            let value = control.args && control.args[1];

                            let node = document.createElement("span");
                            node.innerHTML = `${key}`;

                            node.contentEditable = false;
                            node.dataset.value = key;
                            node.classList.add("merge-tag");
                            node.addEventListener("click", function () {
                                this.editor.s.removeNode(this);
                            });
                            // editor.s.insertHTML(str);
                            editor.s.insertNode(node, true, true);

                            editor.setEditorValue(); // Synchronizing the state of the WYSIWYG editor and the source textarea

                            return false;
                        }
                    }
                    buttons.push('mergeTags');
                }

                if (document.getElementById(id) != null) {
                    var idDoc = "#" + id;
                    console.log("this.editor id", idDoc);
                    this.editor = Jodit.make(idDoc, {
                        uploader: {
                            insertImageAsBase64URI: true
                        },
                        readonly: false,
                        statusbar: true,
                        safeMode: false,
                        buttons: buttons,
                        buttonsXS: buttons,
                        buttonsMD: buttons,
                        buttonsSM: buttons,
                        beautifyHTML: true,
                        hidePoweredByJodit: true
                    });
                    console.log(this.editor);
                    clearInterval(interval);
                    this.editor.e.on("blur", (a) => {
                        console.log("a", a);
                        dotnet.invokeMethodAsync("OnBlur");
                    });
                    let attr = document.body.querySelector(idDoc);
                    var names = attr.getAttributeNames();
                    for (var name in names) {
                        if (names[name].indexOf("b-") > -1) {
                            this.editor.currentPlace.editor.setAttribute(names[name], null);
                        }
                    }
                }
            }
        }, 500);
    }

    GetText(chunk) {
        console.log(this.editor);
        if (this.editor) {
            const textEncoder = new TextEncoder().encode(this.editor.text);
            return textEncoder;
        }
        return "";
    }

    GetHtml(chunk) {
        console.log(this.editor);
        if (this.editor) {
            const textEncoder = new TextEncoder().encode(this.editor.value);
            return textEncoder;
        }

        return "";
    }
}

export function Instance() {
    return new JoditHelper();
}
// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.
export function subscribeOnKeyPress(ref, input) {
    input.onkeydown = function (event) {
        if (event.key == "Enter") {
            event.preventDefault();
            ref.invokeMethodAsync("OnAddTag", null);
        }
    }
}

export function Register(el, ref) {
    console.log("register", el, ref);
    document.addEventListener("click", (e) => onClickReg(e, el, ref));
}

export function UnRegister(el, ref) {
    document.removeEventListener("click", (e) => onClickReg(e, el, ref));
}

function onClickReg(e, el, ref) {
    let isInside = false;
    console.log(el);
    const path = e.composedPath();
    for (let index in path) {
        if (path[index] === el) {
            isInside = true;
            break;
        }
    }
    if (isInside) {
        ref.invokeMethodAsync("OnInsideClick");
    } else {
        ref.invokeMethodAsync("OnOutsideClick");
    }
}
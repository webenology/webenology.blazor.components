class OutsideClick {
    constructor(el, ref) {
        this._el = el;
        this._ref = ref;
        this._binded = this.onClickReg.bind(this);
        console.log(this._ref);
        this.register();
    }
    register() {        
        document.addEventListener("click", this._binded);
    }

    unregister() {
        console.log("unregister", this._ref);
        document.removeEventListener("click", this._binded);
        return true;
    }

    onClickReg(e) {
        let isInside = false;
        const path = e.composedPath();
        for (let index in path) {
            if (path[index] === this._el) {
                isInside = true;
                break;
            }
        }

        console.log("onclick", this._ref, isInside);

        if (this._ref == null)
            return;
        if (isInside) {
            this._ref.invokeMethodAsync("OnInsideClick");
        } else {
            this._ref.invokeMethodAsync("OnOutsideClick");
        }
    }
}

window.OutsideClick = OutsideClick;

export function CreateOutsideClick(el, ref) {
    return new OutsideClick(el, ref);
}

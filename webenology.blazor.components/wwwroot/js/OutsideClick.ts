let isSetup: boolean = false;
const instances: IInstance[] = [];

export function RemoveInstance(el: Element) {
    const instId = el.getAttributeNames().filter(x => x.substr(0, 4) === "_bl_")[0];
    if(Object.isExtensible(this.instances)) {
        this.instances = this.instances.filter(x => x.id !== instId);
    }
}

export function Setup(el: Element, instance: any) {
    const instId = el.getAttributeNames().filter(x => x.substr(0, 4) === "_bl_")[0];

    instances.push({
        id: instId,
        instance
    });

    if (!isSetup) {
        window.addEventListener("click", (e: MouseEvent) => {
            let allEls = [];
            const dfi = document.querySelectorAll("[data-focused-in]");
            dfi.forEach(x => {
                allEls.push(x);
            });

            const path = e.composedPath();
            for (const p of path) {
                if (allEls.indexOf(p) > -1)
                    allEls = allEls.filter(x => x !== p);
            }
            for (const el of allEls) {
                callFocusOut(el);
            }
        });
        isSetup = true;
    }
}

export function SetFocusInAttr(el: Element) {
    el.setAttribute("data-focused-in", "true");
}

function callFocusOut(el: Element) {
    if (el.getAttribute("data-focused-in") === "true") {
        const instId = el.getAttributeNames().filter(x => x.substr(0, 4) === "_bl_")[0];
        const instance = instances.filter(x => x.id === instId)[0].instance;

        el.removeAttribute("data-focused-in");
        instance.invokeMethodAsync("OnClickOutside");
    }
}

interface IInstance {
    instance: any;
    id: string;
}


var isSetup = false;
var instances = [];
export function RemoveInstance(el) {
    var instId = el.getAttributeNames().filter(function (x) { return x.substr(0, 4) === "_bl_"; })[0];
    this.instances = instances.filter(function (x) { return x.id !== instId; });
}
export function Setup(el, instance) {
    var instId = el.getAttributeNames().filter(function (x) { return x.substr(0, 4) === "_bl_"; })[0];
    instances.push({
        id: instId,
        instance: instance
    });
    if (!isSetup) {
        window.addEventListener("click", function (e) {
            var allEls = [];
            var dfi = document.querySelectorAll("[data-focused-in]");
            dfi.forEach(function (x) {
                allEls.push(x);
            });
            var path = e.composedPath();
            var _loop_1 = function (p) {
                if (allEls.indexOf(p) > -1)
                    allEls = allEls.filter(function (x) { return x !== p; });
            };
            for (var _i = 0, path_1 = path; _i < path_1.length; _i++) {
                var p = path_1[_i];
                _loop_1(p);
            }
            for (var _a = 0, allEls_1 = allEls; _a < allEls_1.length; _a++) {
                var el_1 = allEls_1[_a];
                callFocusOut(el_1);
            }
        });
        isSetup = true;
    }
}
export function SetFocusInAttr(el) {
    el.setAttribute("data-focused-in", "true");
}
function callFocusOut(el) {
    if (el.getAttribute("data-focused-in") === "true") {
        var instId_1 = el.getAttributeNames().filter(function (x) { return x.substr(0, 4) === "_bl_"; })[0];
        var instance = instances.filter(function (x) { return x.id === instId_1; })[0].instance;
        el.removeAttribute("data-focused-in");
        instance.invokeMethodAsync("OnClickOutside");
    }
}
//# sourceMappingURL=OutsideClick.js.map
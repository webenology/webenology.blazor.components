export function StopArrows(el) {
    el.addEventListener("keydown", function (e) {
        if (e.key === "ArrowUp" || e.key === "ArrowDown" || e.key === "Enter") {
            e.preventDefault();
        }
    }, false);
}
export function ScrollTo(scrollToEl, count, pixelHeight) {
    var currentHeight = count * pixelHeight;
    scrollToEl.scrollTo(0, currentHeight);
}
//# sourceMappingURL=Combobox.js.map
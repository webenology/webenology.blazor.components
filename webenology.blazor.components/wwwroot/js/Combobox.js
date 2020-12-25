export function StopArrows(el) {
    el.addEventListener("keydown", function (e) {
        if (e.key === "ArrowUp" || e.key === "ArrowDown" || e.key === "Enter") {
            e.preventDefault();
        }
    }, false);
}
export function ScrollTo(scrollToEl, count) {
    var currentHeight = count * 39;
    if (currentHeight < 156)
        currentHeight = currentHeight - 117;
    else
        currentHeight = currentHeight - 78;
    scrollToEl.scrollTo(0, currentHeight);
}
//# sourceMappingURL=Combobox.js.map
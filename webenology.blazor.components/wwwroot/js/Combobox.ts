export function StopArrows(el: Element) {
    el.addEventListener("keydown", (e: any) => {
        if (e.key === "ArrowUp" || e.key === "ArrowDown" || e.key === "Enter") {
            e.preventDefault();
        }
    }, false);
}

export function ScrollTo(scrollToEl: Element, count: number) {
    let currentHeight = count * 39;
    if (currentHeight < 156)
        currentHeight = currentHeight - 117
    else
        currentHeight = currentHeight - 78;

    scrollToEl.scrollTo(0, currentHeight);
}

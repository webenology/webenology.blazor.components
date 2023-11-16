export function StopArrows(el: Element) {
    el.addEventListener("keydown", (e: any) => {
        if (e.key === "ArrowUp" || e.key === "ArrowDown" || e.key === "Enter") {
            e.preventDefault();
        }
    }, false);
}

export function ScrollTo(scrollToEl: Element, count: number, pixelHeight: number) {
    let currentHeight = count * pixelHeight;

    scrollToEl.scrollTo(0, currentHeight);
}

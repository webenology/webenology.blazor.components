export function PositionCalendar(el) {
    var spanBoundingBox = el.getBoundingClientRect();
    var input = el.querySelector(".wc-holder");
    var inputBoundingBox = input.getBoundingClientRect();
    console.log(input.width);
    console.log(input, spanBoundingBox, inputBoundingBox, window.innerWidth);
    var screenWidth = window.innerWidth;

    if (inputBoundingBox.right > screenWidth) {
        var right = spanBoundingBox.width + 20 + spanBoundingBox.left - window.innerWidth;
        input.style.right = `${right}px`;
    } else {
        input.style.right = null;
    }
}
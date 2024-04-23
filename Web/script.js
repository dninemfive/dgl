// https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API
var canvas = document.getElementById("canvas");
// https://stackoverflow.com/a/43525969
// https://kernhanda.github.io/tutorial-typescript-canvas-drawing/
if (!(canvas instanceof HTMLCanvasElement))
    throw new Error("canvas is not an HTMLCanvasElement!");
var context = canvas.getContext("2d");
context.fillStyle = "black";
context.fillRect(10, 10, 10, 10);

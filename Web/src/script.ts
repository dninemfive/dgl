import Board from "./board";
// Canvas API:                      https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API
// Assert type of HTML element:     https://stackoverflow.com/a/43525969
// Draw on a canvas in TS:          https://kernhanda.github.io/tutorial-typescript-canvas-drawing/
// Building document in JS:         https://stackoverflow.com/a/9643489
function renderBoardToCanvas(board: Board, context: CanvasRenderingContext2D): void {
    board.render(context);
}

const canvas: HTMLCanvasElement = document.createElement("canvas");
canvas.height = 200;
canvas.width = 200;
const context = canvas.getContext("2d")
if(!(context instanceof CanvasRenderingContext2D))
    throw new Error("context is not a CanvasRenderingContext2D!");
context.fillStyle = "black"
const startButton = document.createElement("input");
startButton.type = "button";
let board: Board = new Board(200, 200);
startButton.addEventListener("click", () => renderBoardToCanvas(board, context));

document.append(canvas, startButton);
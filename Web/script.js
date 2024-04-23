"use strict";
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
const board_1 = __importDefault(require("./board"));
// Canvas API:                      https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API
// Assert type of HTML element:     https://stackoverflow.com/a/43525969
// Draw on a canvas in TS:          https://kernhanda.github.io/tutorial-typescript-canvas-drawing/
// Building document in JS:         https://stackoverflow.com/a/9643489
function renderBoardToCanvas(board, context) {
    board.render(context);
}
const canvas = document.createElement("canvas");
canvas.height = 200;
canvas.width = 200;
const context = canvas.getContext("2d");
if (!(context instanceof CanvasRenderingContext2D))
    throw new Error("context is not a CanvasRenderingContext2D!");
context.fillStyle = "black";
const startButton = document.createElement("input");
startButton.type = "button";
let board = new board_1.default(200, 200);
startButton.addEventListener("click", () => renderBoardToCanvas(board, context));
document.append(canvas, startButton);

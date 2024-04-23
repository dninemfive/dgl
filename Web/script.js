"use strict";
// https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API
const canvas = document.getElementById("canvas");
// https://stackoverflow.com/a/43525969
// https://kernhanda.github.io/tutorial-typescript-canvas-drawing/
if (!(canvas instanceof HTMLCanvasElement))
    throw new Error("canvas is not an HTMLCanvasElement!");
const context = canvas.getContext("2d");
if (!(context instanceof CanvasRenderingContext2D))
    throw new Error("context is not a CanvasRenderingContext2D!");
context.fillStyle = "black";
class Board {
    constructor(width, height) {
        // https://stackoverflow.com/a/47801159
        this._board = new Array();
        for (let x = 0; x < width; x++) {
            this._board[x] = new Array();
            for (let y = 0; y < height; y++) {
                this._board[x][y] = false;
            }
        }
    }
    get width() {
        return this._board.length;
    }
    get height() {
        return this._board[0].length;
    }
    render(context) {
        // avoid (permanently) mutating the context
        let originalFillStyle = context.fillStyle;
        let canvas = context.canvas;
        if (canvas.width < this.width || canvas.height < this.height) {
            throw new Error(`Tried to render a board of dimension ${this.width}x${this.height} on a canvas of dimension ${canvas.width}x${canvas.height}!`);
        }
        for (let x = 0; x < this.width; x++) {
            for (let y = 0; y < this.height; y++) {
                context.fillStyle = this._board[x][y] ? "black" : "magenta";
                context.fillRect(x, y, 1, 1);
            }
        }
        context.fillStyle = originalFillStyle;
    }
}
let board = new Board(200, 200);
// todo: context as an argument
function test() {
    if (!(context instanceof CanvasRenderingContext2D))
        throw new Error("test(): context is not a CanvasRenderingContext2D!");
    board.render(context);
}

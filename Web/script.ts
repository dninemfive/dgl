// https://developer.mozilla.org/en-US/docs/Web/API/Canvas_API
const canvas = document.getElementById("canvas");
// https://stackoverflow.com/a/43525969
// https://kernhanda.github.io/tutorial-typescript-canvas-drawing/
if(!(canvas instanceof HTMLCanvasElement))
    throw new Error("canvas is not an HTMLCanvasElement!")
let context: CanvasRenderingContext2D = canvas.getContext("2d")
context.fillStyle = "black"

class Board {
    private _board: boolean[][];
    constructor(width: number, height: number) {
        // https://stackoverflow.com/a/47801159
        this._board = new Array<Array<boolean>>();
        for(let x: number = 0; x < width; x++) {
            this._board[x] = new Array<boolean>();
            for(let y: number = 0; y < height; y++) {
                this._board[x][y] = false;
            }
        }
    }
    width(): number {
        return this._board.length;
    }
    height(): number {
        return this._board[0].length;
    }
    render(context: CanvasRenderingContext2D): void {
        // avoid (permanently) mutating the context
        let originalFillStyle: string|CanvasGradient|CanvasPattern = context.fillStyle;
        let canvas: HTMLCanvasElement = context.canvas;
        if(canvas.width < this.width() || canvas.height < this.height()) {
            throw new Error(`Tried to render a board of dimension ${this.width()}x${this.height()} on a canvas of dimension ${canvas.width}x${canvas.height}!`);
        }
        for(let x: number = 0; x < this.width(); x++) {
            for(let y: number = 0; y < this.height(); y++) {
                context.fillStyle = this._board[x][y] ? "black" : "white";
                context.fillRect(x, y, 1, 1)
            }
        }
        context.fillStyle = originalFillStyle;
    }
}
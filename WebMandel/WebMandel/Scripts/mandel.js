"use strict;"

function MandelStackEntry(xMin, yMax, scale, iterations, image) {
    this.xMin = xMin;
    this.yMax = yMax;
    this.scale = scale;
    this.iterations = iterations;
    this.image = image;
};

function MandelStack(imgUrl) {
    var stack = [];
    var pos = -1;
    var firstImage = true;
    var composeImageUrl = function (xMin, yMax, scale, iterations) {
        return imgUrl + '?' + 'xMin=' + xMin + '&yMax=' + yMax + '&scale=' + scale + '&iterations=' + iterations;
    };
    var bltImage = function (entry) {
        var canv = $('#mandelImage');
        var ctx = canv[0].getContext('2d');
        if (firstImage) {
            ctx.canvas.width = entry.image.width;
            ctx.canvas.height = entry.image.height;
            firstImage = false;
        }
        ctx.drawImage(entry.image, 0, 0);
        $('#mandelCaption').text('xMin = ' + entry.xMin + ', yMax = ' + entry.yMax + ', scale = ' + entry.scale)
        endLoading();
    };
    this.push = function (xMin, yMax, scale, iterations) {
        stack.length = pos++ + 2;
        var image = new Image();
        var entry = new MandelStackEntry(xMin, yMax, scale, iterations, image);
        stack[pos] = entry;
        beginLoading();
        image.onload = function () { bltImage(entry); };
        image.src = composeImageUrl(xMin, yMax, scale, iterations);
        $('#backButton').prop('disabled', pos < 1);
        $('#forwardButton').prop('disabled', true);
    };
    this.back = function () {
        if (pos > 0) {
            bltImage(stack[--pos]);
            $('#forwardButton').prop('disabled', false);
        }
        if (pos == 0) {
            $('#backButton').prop('disabled', true);
        }
    };
    this.forward = function () {
        if (pos < stack.length - 1) {
            bltImage(stack[++pos]);
            $('#backButton').prop('disabled', false);
        }
        if (pos == stack.length - 1) {
            $('#forwardButton').prop('disabled', true);
        }
    };
    this.xMin = function () {
        return stack[pos].xMin;
    };
    this.yMax = function () {
        return stack[pos].yMax;
    };
    this.scale = function () {
        return stack[pos].scale;
    }
    this.iterations = function () {
        return stack[pos].iterations;
    }
    this.image = function () {
        return stack[pos].image;
    }
};

var stack;
var dragging;
var startPoint;

function getMousePos(canvas, evt) {
    var rect = canvas.getBoundingClientRect();
    return {
        x: evt.clientX - rect.left,
        y: evt.clientY - rect.top
    };
}

function setupContext(canvas) {
    var ctx = canvas.getContext('2d');
    ctx.strokeStyle = "blue";
    return ctx;
}

function drawRect(ctx, rect) {
    ctx.drawImage(stack.image(), 0, 0);
    ctx.strokeRect(rect.p1.x, rect.p1.y, rect.p2.x - rect.p1.x, rect.p2.y - rect.p1.y);
}

function onMouseDown(evt) {
    if (evt.button != 0) { return; }
    dragging = true;
    var canvas = $(this)[0];
    startPoint = getMousePos(canvas, evt);
}

function computeNewCoordinates(canvas, start, end) {
    var xMin = stack.xMin();
    var yMax = stack.yMax();
    var scale = stack.scale();
    var newXMin = xMin + start.x * scale;
    var newYMax = yMax - start.y * scale;
    var newXMax = xMin + end.x * scale;
    var newYMin = yMax - end.y * scale;
    var newScaleX = (newXMax - newXMin) / canvas.width;
    var newScaleY = (newYMax - newYMin) / canvas.height;
    stack.push(newXMin, newYMax, Math.max(newScaleX, newScaleY), stack.iterations());
}

function onMouseUp(evt) {
    if (!dragging) return;
    dragging = false;
    var canvas = $(this)[0];
    var ctx = setupContext(canvas);
    ctx.drawImage(stack.image(), 0, 0);
    var endPoint = getMousePos(canvas, evt);
    if (endPoint.x <= startPoint.x || endPoint.y <= startPoint.y) return;
    computeNewCoordinates(canvas, startPoint, endPoint);
}

function onMouseLeave(evt) {
    if (!dragging) return;
    dragging = false;
    var canvas = $(this)[0];
    var ctx = setupContext(canvas);
    ctx.drawImage(stack.image(), 0, 0);
}

function onMouseMove(evt) {
    if (!dragging) return;
    var canvas = $(this)[0];
    var pos = getMousePos(canvas, evt);
    var ctx = setupContext(canvas);
    var rect = {
        p1: startPoint,
        p2: pos
    };
    drawRect(ctx, rect);
}

function setupCanvas(canvas) {
    dragging = false;
    canvas.mousedown(onMouseDown);
    canvas.mouseup(onMouseUp);
    canvas.mouseleave(onMouseLeave);
    canvas.mousemove(onMouseMove);
}

function beginLoading() {
    $('#mandelProgressLabel').text('Generating image...');
    $('#mandelProgress').progressbar({
        value: false
    });
}

function endLoading() {
    $('#mandelProgressLabel').text('Ready.');
    $('#mandelProgress').progressbar("option", "value", 0);
    $('#Iterations').val(stack.iterations());
}

function getFirstImg(imgUrl, xMin, yMax, scale, iterations) {
    setupCanvas($('#mandelImage'));
    stack = new MandelStack(imgUrl);
    stack.push(xMin, yMax, scale, iterations);
}

function onRecompute(evt) {
    var form = $('#mandelForm');
    evt.originalEvent.preventDefault();
    if (!form.valid()) return false;
    var newIterations = parseInt($('#Iterations').val());
    if (newIterations != stack.iterations()) {
        stack.push(stack.xMin(), stack.yMax(), stack.scale(), newIterations);
    }
    return false;
}

function onForward() {
    stack.forward();
}

function onBack() {
    stack.back();
}
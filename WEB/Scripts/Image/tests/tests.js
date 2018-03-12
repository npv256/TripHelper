/*
 * ©2016 Quicken Loans Inc. All rights reserved.
 */
/* global describe it File */
var should = require('should');
var jsdom = require('jsdom');
var fs = require('fs');
var sinon = require('sinon');

var htmlSource = fs.readFileSync('./tests/html-stub.html', 'utf8');
var document = jsdom.jsdom(htmlSource);
var window = document.defaultView;
window.addEventListener('error', function (event) {
    console.error('script error!!', event.error);
});

require('jquery')(window);

window.console = console;

var scriptEl = window.document.createElement('script');
scriptEl.innerHTML = fs.readFileSync('./dist/jquery.imageuploader.js', 'utf8');
window.document.body.appendChild(scriptEl);

global.document = document;
global.window = window;

// for when people don't write window in front of browser code
propagateToGlobal(window);
function propagateToGlobal (window) {
    for (var key in window) {
        if (!window.hasOwnProperty(key)) continue;
        if (key in global) continue;
        global[key] = window[key];
    }
}

describe('Setup', function () {
    describe('#clearout', function () {
        // set up the uploader
        window.$('.js-uploader__box').uploader({testMode: true});
        it('should have the required container still', function () {
            should.equal(1, window.$('.js-uploader__box').length);
        });
        it('should not have the stuff that was in there at first', function () {
            should.equal(0, window.$('.js-fallback-form').length);
        });
    });
    describe('#dom-elements', function () {
        it('should have added one submit button', function () {
            should.equal(1, window.$('.js-uploader__submit-button').length);
        });
        it('should have added the select button (label and input)', function () {
            should.equal(1, window.$('.js-uploader__box .js-uploader__file-input').length);
            should.equal(1, window.$('.js-uploader__box .js-uploader__file-label').length);
        });
        it('should have added further instructions', function () {
            should.equal(1, window.$('.js-uploader__further-instructions').length);
        });
        it('should have added the file list', function () {
            should.equal(1, window.$('.js-uploader__file-list').length);
        });
    });
});

describe('Drag and Drop', function () {
    describe('#drop', function () {
        it('should add a file list item if a good file is dropped', function () {
            // 'drop' a good file
            var dropEventOneFile = window.$.Event('uploaderTestEvent');
            dropEventOneFile.functionName = 'selectFilesHandler';
            dropEventOneFile.target = {};
            dropEventOneFile.target.files = [new File([], 'test.jpg', {})];
            window.$('.js-uploader__box').trigger(dropEventOneFile);
            should.equal(1, window.$('.js-uploader__file-list').children().length);
        });
        it('should add a file list item if a bad file is dropped', function () {
            // clear the list
            window.$('.js-uploader__file-list').empty();
            // 'drop' a bad file
            var dropEventOneFile = window.$.Event('uploaderTestEvent');
            dropEventOneFile.functionName = 'selectFilesHandler';
            dropEventOneFile.target = {};
            dropEventOneFile.target.files = [new File([], 'test.csv', {})];
            window.$('.js-uploader__box').trigger(dropEventOneFile);
            should.equal(1, window.$('.js-uploader__file-list').children().length);
        });
        it('should add a file list item for each, if a set of good files is dropped', function () {
            window.$('.js-uploader__file-list').empty();
            var dropEventManyGoodFiles = window.$.Event('uploaderTestEvent');
            dropEventManyGoodFiles.functionName = 'selectFilesHandler';
            dropEventManyGoodFiles.target = {};
            dropEventManyGoodFiles.target.files = [
                new File([], 'test.jpg', {}),
                new File([], 'test.jpg', {}),
                new File([], 'test.jpg', {}),
                new File([], 'test.jpg', {}),
                new File([], 'test.jpg', {})
            ];
            window.$('.js-uploader__box').trigger(dropEventManyGoodFiles);
            should.equal(5, window.$('.js-uploader__file-list').children().length);
        });
        it('should add a file list item for each file, if a set of bad files is dropped', function () {
            window.$('.js-uploader__file-list').empty();
            var dropEventManyBadFiles = window.$.Event('uploaderTestEvent');
            dropEventManyBadFiles.functionName = 'selectFilesHandler';
            dropEventManyBadFiles.target = {};
            dropEventManyBadFiles.target.files = [
                new File([], 'test.csv', {}),
                new File([], 'test.csv', {}),
                new File([], 'test.csv', {}),
                new File([], 'test.csv', {}),
                new File([], 'test.csv', {})
            ];
            window.$('.js-uploader__box').trigger(dropEventManyBadFiles);
            should.equal(5, window.$('.js-uploader__file-list').children().length);
        });
        it('should add a file list item for each bad one and for each good one,  if a set of bad and good files is dropped', function () {
            window.$('.js-uploader__file-list').empty();
            var dropEventManyFiles = window.$.Event('uploaderTestEvent');
            dropEventManyFiles.functionName = 'selectFilesHandler';
            dropEventManyFiles.target = {};
            dropEventManyFiles.target.files = [
                new File([], 'test.csv', {}),
                new File([], 'test.jpg', {}),
                new File([], 'test.csv', {}),
                new File([], 'test.jpg', {}),
                new File([], 'test.jpg', {})
            ];
            window.$('.js-uploader__box').trigger(dropEventManyFiles);
            should.equal(5, window.$('.js-uploader__file-list').children().length);
        });
    });
});

describe('Upload Submit', function () {
    describe('#uploadsubmit', function () {
        it('should make an ajax request with the files as formdata', function () {
            // select some files
            var dropEventManyFiles = window.$.Event('uploaderTestEvent');
            dropEventManyFiles.functionName = 'selectFilesHandler';
            dropEventManyFiles.target = {};
            var fileString = [''];
            for (var i = 0; i < 50000; i++) {
                fileString[0] += 'Lorem ipsum dolor sit amet something something';
            }

            for (var j = 0; j < 25; j++) {
                fileString.push(fileString[0]);
            }

            var file = new File(fileString, 'test.jpg', {});
            dropEventManyFiles.target.files = [file];
            window.$('.js-uploader__box').trigger(dropEventManyFiles);

            sinon.spy(window.$, 'ajax');

            // trigger upload submit
            var submitUpload = window.$.Event('uploaderTestEvent');
            submitUpload.functionName = 'uploadSubmitHandler';
            window.$('.js-uploader__box').trigger(submitUpload);
            should.equal(true, window.$.ajax.calledOnce);
            window.$.ajax.restore();
        });
    });
});

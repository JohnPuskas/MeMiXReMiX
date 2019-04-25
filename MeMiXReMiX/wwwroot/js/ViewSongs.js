try {
    var AudioContext = window.AudioContext || window.webkitAudioContext;
    var audioCtx = new AudioContext();
} catch (e) {
    alert("Web Audio API not supported in this browser.")
}

let mp3Urls = new Array();
let requestedBuffers = {};
let decodedBuffers = {};
let currentlyPlayingInstruments = {};

//To be concatenated into full file paths/name
var drumURLSegment = "/media/music/Drums/Drums";
var bassURLSegment = "/media/music/Bass/Bass";
var chordsURLSegment = "/media/music/Chords/Chords";
var melodicURLSegment = "/media/music/Melodic/Melodic";

var instrumentUrlSegments = [drumURLSegment, bassURLSegment, chordsURLSegment, melodicURLSegment];


function songPlayer(filePointer, stopID) {

    if (audioCtx.state === 'suspended') {
        audioCtx.resume();
    };

    var stopSong = document.getElementById(stopID);

    var playButtons = document.getElementsByClassName('play');
    /*Need to decrement loop because document.getElementsByClassName provides an
    'array-like object' that removes any altered element from itself. */
    for (var i = playButtons.length - 1; i >= 0; i--) {
        playButtons[i].disabled = true;
    };
    progressBarID = ("progressBar-" + stopID);
    var progressBar = document.getElementById(progressBarID);
    progressBar.hidden = false;

    // For counting the number off XHRs that have reached onload event
    var bufferLoadedCounter = 0;

    for (var i = 0; i < filePointer.length; i++) {
        mp3Urls[i] = instrumentUrlSegments[i] + filePointer[i] + ".mp3";

        // This function captures the value of 'i' so that code following onload corresponds to right XHR
        (function (i) {

            requestedBuffers[mp3Urls[i]] = new XMLHttpRequest();
            requestedBuffers[mp3Urls[i]].open("GET", mp3Urls[i], true);
            requestedBuffers[mp3Urls[i]].responseType = "arraybuffer";
            requestedBuffers[mp3Urls[i]].onload = function () {
                audioCtx.decodeAudioData(requestedBuffers[mp3Urls[i]].response, function (buffer) {
                    decodedBuffers[mp3Urls[i]] = buffer;

                    bufferLoadedCounter++;

                    // Upon all XHR reaching onload, song plays
                    if (bufferLoadedCounter == filePointer.length) {

                        playSong(filePointer);

                        progressBar.hidden = true;
                        stopSong.disabled = false;
                    }
                });
            }
            requestedBuffers[mp3Urls[i]].send();
        })(i);

    };
}

function playSong(pointerForLoopLength) {
    for (var j = 0; j < pointerForLoopLength.length; j++) {
        var audioSource = audioCtx.createBufferSource();
        audioSource.buffer = decodedBuffers[mp3Urls[j]];
        audioSource.connect(audioCtx.destination);
        audioSource.loop = true;
        audioSource.start(0);

        currentlyPlayingInstruments[mp3Urls[j]] = audioSource; //need this dictionary to stop all tracks in song
    }
}

function stopPlayingSong(stopButtonID) {

    var stopButton = document.getElementById(stopButtonID);

    stopButton.disabled = true;

    var playButtons = document.getElementsByClassName('play');

    for (var h = playButtons.length - 1; h >= 0; h--) {
        playButtons[h].disabled = false;
    };


    for (var j = 0; j < mp3Urls.length; j++) {
        currentlyPlayingInstruments[mp3Urls[j]].stop();
    }
}


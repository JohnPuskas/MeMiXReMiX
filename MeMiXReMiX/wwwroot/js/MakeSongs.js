var isAudioContext = false;
var audioCtx;

let buffers = {};
let offset = null;
let playingSelection = null;
let currentlyPlayingInstruments = {};

// To be concatenated into full file path/name
var drumURLSegment = "/media/music/Drums/Drums";
var bassURLSegment = "/media/music/Bass/Bass";
var chordsURLSegment = "/media/music/Chords/Chords";
var melodicURLSegment = "/media/music/Melodic/Melodic";

var instrumentUrlSegments = [drumURLSegment, bassURLSegment, chordsURLSegment, melodicURLSegment];


/*
    Grabs the appropriate buffer from the buffers dictionary, using the URL as the Key.
    Stores the audioSource in a dictionary with the URL segment (that contains the instrument type) as the key.
    When User adds a new mp3 to the song, the dictionary is referenced to stop the already playing mp3 of that instrument type.
 */
function playInstrument(instrumentNumber, selectorID, playButtonId) {
    var playButton = document.getElementById(playButtonId);
    playButton.disabled = true;

    // Gets the file path/name of file that was selected by user (and thus loaded & then stored in dictionary by getMp3 function)
    var e = document.getElementById(selectorID);
    pickedFileNumber = e.options[e.selectedIndex].value;
    var playUrl = instrumentUrlSegments[instrumentNumber] + pickedFileNumber + ".mp3";

    var playBuffer = buffers[playUrl];

    // Prepares AudioBuffer for playing
    var audioSource = audioCtx.createBufferSource();
    audioSource.buffer = playBuffer;
    audioSource.connect(audioCtx.destination);
    audioSource.loop = true;

    // If this is the 1st track played, the offset would still be 0, so it starts at the beginning of the audio loop.
    if (offset == null) {
        audioSource.start(0);

        // Reassigns value of offset, so it can be used for calculating the starting point within subsequently played audio loops.
        offset = audioCtx.currentTime;

        // Used for stopping the audio loop when another audio loop of the same instrument type is played. Or when 'Stop Music' is clicked.
        playingSelection = audioSource;
        var playingFileURLSegment = instrumentUrlSegments[instrumentNumber];
        currentlyPlayingInstruments[playingFileURLSegment] = playingSelection;

        var stopButton = document.getElementById("stop-song");
        stopButton.disabled = false;

    } else {

        var playingFileURLSegment = instrumentUrlSegments[instrumentNumber];

        // Stops an audio loop from playing if it is the same instrument type as a new audio loop that is user-selected for playback 
        if (currentlyPlayingInstruments[playingFileURLSegment]) {

            var currentInstrumentAudio = currentlyPlayingInstruments[playingFileURLSegment];
            currentInstrumentAudio.disconnect();
        };

        // Starts the audio loop synced up with the audio loops that are already playing.
        var trackLength = playBuffer.duration;
        var startingPlace = (audioCtx.currentTime - offset) % trackLength;

        audioSource.start(audioCtx.currentTime, startingPlace);

        // Used for stopping the audio loop when another audio loop of the same instrument type is played. Or when 'Stop Music' is clicked.
        playingSelection = audioSource;
        currentlyPlayingInstruments[playingFileURLSegment] = playingSelection;
    };
}

function stopAllTracks() {
    for (var key in currentlyPlayingInstruments) {
        currentlyPlayingInstruments[key].disconnect();
    };

    offset = null;

    var stopButton = document.getElementById("stop-song");
    stopButton.disabled = true;

    var playButtons = document.getElementsByClassName("play-track");
    var selectors = document.getElementsByClassName('selectors');

    /*Need to decrement loop because document.getElementsByClassName provides an 
    'array-like object' that removes any altered element from itself. */
    for (var i = playButtons.length - 1; i >= 0; i--) {
        if (selectors[i].value != 0) {
            playButtons[i].disabled = false;
        }
    };
}

/*
Uses function parameters to create the appropriate mp3 URL, then get the file for playback.
Then stores the buffer in a dictionary which can be accessed when the play buttons are clicked.
'instrumentNumber' is hard-coded in the HTML for each instrument type.
 'pickedFileNumber' is the int value associated with the User selection from the instrument type drop-down.
*/
function getMp3(instrumentNumber, selectorID, playButtonId) {

    if (isAudioContext == false) {

        try {
            var AudioContext = window.AudioContext || window.webkitAudioContext;
            audioCtx = new AudioContext();
        } catch (e) {
            alert("Web Audio API not supported in this browser.")
        }
        isAudioContext = true;
    }

    var e = document.getElementById(selectorID);
    pickedFileNumber = e.options[e.selectedIndex].value;
    var mp3Url = instrumentUrlSegments[instrumentNumber] + pickedFileNumber + ".mp3";

    var playButton = document.getElementById(playButtonId);
    playButton.disabled = true;
    if (pickedFileNumber != 0) {
        playButton.innerHTML = "Loading...";
    }

    /* XHR for retrieving data that is then decoded with the Web Audio API decodeAudioData method from an ArrayBuffer into an AudioBuffer.
     * With the decodeAudioData method, the AudioBuffer is passed into the function that is placed as its second argument. */
    var requestedAudio = new XMLHttpRequest();
    requestedAudio.open("GET", mp3Url, true);
    requestedAudio.responseType = "arraybuffer";
    requestedAudio.onload = function () {
        audioCtx.decodeAudioData(requestedAudio.response, storeRequestedAudio)

    }
    requestedAudio.send();

    // Store AudioBuffer in a dictionary that is accessed by the playInstrument function.    
    function storeRequestedAudio(buffer) {
        buffers[mp3Url] = buffer;
        if (pickedFileNumber != 0) {
            playButton.disabled = false;

            if (playButtonId == 'play-drums') {
                playButton.innerHTML = 'Play Drums';
            } else if (playButtonId == 'play-bass') {
                playButton.innerHTML = 'Play Bass';
            } else if (playButtonId == 'play-chords') {
                playButton.innerHTML = 'Play Chords';
            } else if (playButtonId == 'play-melodic') {
                playButton.innerHTML = 'Play Melodic';
            }

        }
    }
}

/*
 Upon clicking 'submit', takes all of the numerical values of User selections and
 puts them in string to be stored in DB. THEN submits the form.
*/
function collectSelections() {

    var songTitle = document.getElementById('title');

    if (songTitle.value.trim() == '') {

        alert("Oops! Looks like your song needs a title.")

    } else {

        var fileDigits = document.getElementById("digits");
        fileDigits.value = "";

        var dropdownElements = document.getElementsByClassName("selectors");
        for (i = 0; i < dropdownElements.length; i++) {
            dropdownValue = dropdownElements[i].value;
            fileDigits.value = fileDigits.value + dropdownValue;

        }

        if (fileDigits.value.includes(0)) {

            alert("Please fill your mix selections before saving.");

        } else {

            document.getElementById("actual-submit").click();

        };

    };
}


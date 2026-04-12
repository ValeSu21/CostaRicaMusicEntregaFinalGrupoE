(function () {
    const STORAGE_KEY = 'crmusic-player-state';

    const audio = document.getElementById('audioPlayer');
    const title = document.getElementById('playerTitle');
    const subtitle = document.getElementById('playerSubtitle');
    const cover = document.getElementById('playerCover');
    const btnPrev = document.getElementById('btnPrev');
    const btnPlayPause = document.getElementById('btnPlayPause');
    const btnNext = document.getElementById('btnNext');

    if (!audio || !title || !subtitle || !cover || !btnPrev || !btnPlayPause || !btnNext) {
        return;
    }

    let state = loadState();
    let queue = Array.isArray(state.queue) ? state.queue : collectQueueFromPage();
    let restoringTime = false;

    if (!Array.isArray(state.queue) || !state.queue.length) {
        state.queue = queue;
        saveState();
    }

    hydrateFromState();
    wireSongButtons();

    btnPlayPause.addEventListener('click', togglePlayPause);
    btnPrev.addEventListener('click', playPrevious);
    btnNext.addEventListener('click', playNext);
    audio.addEventListener('ended', playNext);

    audio.addEventListener('timeupdate', function () {
        state.currentTime = audio.currentTime || 0;
        saveState();
    });

    audio.addEventListener('play', function () {
        state.isPlaying = true;
        updateButtonState();
        saveState();
    });

    audio.addEventListener('pause', function () {
        state.isPlaying = false;
        updateButtonState();
        saveState();
    });

    window.addEventListener('beforeunload', function () {
        state.currentTime = audio.currentTime || 0;
        state.isPlaying = !audio.paused;
        saveState();
    });

    function collectQueueFromPage() {
        const buttons = document.querySelectorAll('.play-song');
        const unique = [];
        const seen = new Set();

        buttons.forEach(button => {
            const song = parseSong(button);
            if (!song || seen.has(song.id)) {
                return;
            }

            seen.add(song.id);
            unique.push(song);
        });

        return unique;
    }

    function wireSongButtons() {
        const buttons = document.querySelectorAll('.play-song');

        buttons.forEach(button => {
            button.addEventListener('click', function () {
                queue = collectQueueFromPage();
                state.queue = queue;

                const song = parseSong(button);
                if (!song) return;

                state.currentIndex = queue.findIndex(x => x.id === song.id);
                if (state.currentIndex < 0) state.currentIndex = 0;

                state.currentTime = 0;
                loadSong(state.queue[state.currentIndex], true, true);
            });
        });
    }

    function parseSong(button) {
        const id = Number(button.dataset.id || 0);

        const song = {
            id,
            title: button.dataset.title || 'Sin título',
            artist: button.dataset.artist || 'Sin artista',
            audio: button.dataset.audio || '',
            cover: button.dataset.cover || '/media/images/canciones/default-cover.svg'
        };

        if (!song.id || !song.audio) return null;
        return song;
    }

    function hydrateFromState() {
        if (!state.queue?.length) {
            state.queue = queue;
            saveState();
        }

        if (
            state.queue?.length &&
            typeof state.currentIndex === 'number' &&
            state.currentIndex >= 0 &&
            state.currentIndex < state.queue.length
        ) {
            const current = state.queue[state.currentIndex];
            if (current) {
                loadSong(current, state.isPlaying, false);
            }
        }

        updateButtonState();
    }

    function loadSong(song, shouldAutoplay, resetTime) {
        if (!song) return;

        title.textContent = song.title;
        subtitle.textContent = song.artist;
        cover.src = song.cover;

        const targetUrl = new URL(song.audio, window.location.origin).href;
        const sourceChanged = audio.src !== targetUrl;

        if (sourceChanged) {
            audio.src = song.audio;
        }

        if (resetTime) {
            state.currentTime = 0;
        }

        saveState();
        updateButtonState();

        if (sourceChanged) {
            restoringTime = true;

            audio.addEventListener('loadedmetadata', handleMetadataLoaded, { once: true });
        } else {
            restoreTimeAndMaybePlay(shouldAutoplay);
        }
    }

    function handleMetadataLoaded() {
        restoreTimeAndMaybePlay(state.isPlaying);
    }

    function restoreTimeAndMaybePlay(shouldAutoplay) {
        if (restoringTime) {
            const savedTime = Number(state.currentTime || 0);

            if (!Number.isNaN(savedTime) && savedTime > 0 && savedTime < audio.duration) {
                try {
                    audio.currentTime = savedTime;
                } catch (_) {
                }
            }

            restoringTime = false;
        }

        if (shouldAutoplay) {
            audio.play().then(() => {
                state.isPlaying = true;
                updateButtonState();
                saveState();
            }).catch(() => {
                state.isPlaying = false;
                updateButtonState();
                saveState();
            });
        } else {
            updateButtonState();
        }
    }

    function togglePlayPause() {
        if (!state.queue?.length) {
            queue = collectQueueFromPage();
            state.queue = queue;
            state.currentIndex = 0;

            if (!queue.length) return;

            state.currentTime = 0;
            loadSong(queue[0], true, true);
            return;
        }

        if (!audio.src) {
            loadSong(state.queue[state.currentIndex || 0], true, false);
            return;
        }

        if (audio.paused) {
            audio.play().then(() => {
                state.isPlaying = true;
                updateButtonState();
                saveState();
            }).catch(() => {
            });
        } else {
            audio.pause();
            state.isPlaying = false;
            updateButtonState();
            saveState();
        }
    }

    function playPrevious() {
        if (!state.queue?.length) return;

        state.currentIndex = (state.currentIndex - 1 + state.queue.length) % state.queue.length;
        state.currentTime = 0;
        loadSong(state.queue[state.currentIndex], true, true);
    }

    function playNext() {
        if (!state.queue?.length) return;

        state.currentIndex = (state.currentIndex + 1) % state.queue.length;
        state.currentTime = 0;
        loadSong(state.queue[state.currentIndex], true, true);
    }

    function updateButtonState() {
        btnPlayPause.textContent = (!audio.paused && !!audio.src) ? 'Pausa' : 'Play';
    }

    function loadState() {
        try {
            return JSON.parse(localStorage.getItem(STORAGE_KEY)) || {
                queue: [],
                currentIndex: 0,
                isPlaying: false,
                currentTime: 0
            };
        } catch {
            return {
                queue: [],
                currentIndex: 0,
                isPlaying: false,
                currentTime: 0
            };
        }
    }

    function saveState() {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(state));
    }

    window.playSong = function (id, songTitle, artist, audioUrl, coverUrl) {
        queue = collectQueueFromPage();
        state.queue = queue;

        state.currentIndex = queue.findIndex(x => x.id === Number(id));

        if (state.currentIndex < 0) {
            const injected = {
                id: Number(id),
                title: songTitle,
                artist,
                audio: audioUrl,
                cover: coverUrl || '/media/images/canciones/default-cover.svg'
            };

            state.queue.push(injected);
            state.currentIndex = state.queue.length - 1;
        }

        state.currentTime = 0;
        loadSong(state.queue[state.currentIndex], true, true);
    };
})();
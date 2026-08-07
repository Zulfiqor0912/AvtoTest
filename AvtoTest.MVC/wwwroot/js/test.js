// ========== TIMER ==========
let timeLeft = 20 * 60;
const timerDisplay = document.getElementById('timeDisplay');
const timerContainer = document.getElementById('timer');

function updateTimer() {
    const minutes = Math.floor(timeLeft / 60);
    const seconds = timeLeft % 60;
    timerDisplay.textContent = `${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;

    if (timeLeft <= 0) {
        clearInterval(timerInterval);
        alert('⏰ Vaqt tugadi! Test avtomatik yakunlandi.');
        window.location.href = '/Home/TestResult?ticketId=' + ticketId;
    }

    timerContainer.classList.remove('pulsing', 'warning');

    if (timeLeft <= 60) {
        timerContainer.classList.add('pulsing');
        timerDisplay.style.color = '#dc3545';
    } else if (timeLeft <= 300) {
        timerContainer.classList.add('warning');
        timerDisplay.style.color = '#ffc107';
    } else {
        timerDisplay.style.color = '#1a2634';
    }

    timeLeft--;
}

let timerInterval = setInterval(updateTimer, 1000);

// ========== STATS ==========
function updateStats() {
    const links = document.querySelectorAll('.pagination-wrapper .page-link');
    let correct = 0, wrong = 0, empty = 0;

    links.forEach(link => {
        if (link.classList.contains('active')) {
            return;
        }

        if (link.classList.contains('correct')) {
            correct++;
        } else if (link.classList.contains('wrong')) {
            wrong++;
        } else {
            empty++;
        }
    });

    const correctEl = document.getElementById('correctCount');
    const wrongEl = document.getElementById('wrongCount');
    const emptyEl = document.getElementById('emptyCount');

    if (correctEl) correctEl.textContent = correct;
    if (wrongEl) wrongEl.textContent = wrong;
    if (emptyEl) emptyEl.textContent = empty;

    console.log('Stats updated:', { correct, wrong, empty });
}

// ========== SAHIFA YUKLANGANDA ==========
document.addEventListener('DOMContentLoaded', function () {
    setTimeout(updateStats, 200);

    const navLinks = document.querySelectorAll('.pagination-wrapper .page-link, .pagination-wrapper .page-nav, .nav-btn');
    navLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            setTimeout(updateStats, 500);
        });
    });

    const choiceBtns = document.querySelectorAll('.choice-btn');
    choiceBtns.forEach(function (btn) {
        btn.addEventListener('click', function () {
            setTimeout(updateStats, 500);
        });
    });
});

// ========== KEYBOARD SHORTCUTS ==========
document.addEventListener('keydown', function (e) {
    const choiceButtons = document.querySelectorAll('.choice-btn:not([disabled])');
    const keyMap = {
        '1': 0,
        '2': 1,
        '3': 2,
        '4': 3
    };

    if (e.key in keyMap && choiceButtons[keyMap[e.key]]) {
        e.preventDefault();
        choiceButtons[keyMap[e.key]].click();
    }

    if (e.key === 'ArrowLeft') {
        const prevBtn = document.querySelector('.nav-btn.prev');
        if (prevBtn) {
            e.preventDefault();
            prevBtn.click();
        }
    }
    if (e.key === 'ArrowRight') {
        const nextBtn = document.querySelector('.nav-btn.next');
        if (nextBtn) {
            e.preventDefault();
            nextBtn.click();
        }
    }
});

// ========== FINISH TEST ==========
function finishTest() {
    clearInterval(timerInterval);
    updateStats();
    // Test tugadi
}

// ========== EXPOSE FUNCTIONS ==========
window.finishTest = finishTest;
window.updateStats = updateStats;

console.log('Test.js loaded successfully!');

document.addEventListener("DOMContentLoaded", () => {
    const emberLayer = document.getElementById("ember-layer");
    const menuLinks = document.querySelectorAll(".fiery-link");

    const reduceMotion = window.matchMedia(
        "(prefers-reduced-motion: reduce)"
    ).matches;

    // ==========================================
    // 1. KOOPA TAUSTAL HÕLJUVAD SÄDEMED
    // ==========================================

    function createEmber() {
        if (!emberLayer) return;

        const ember = document.createElement("span");
        ember.className = "ember";

        const size = Math.random() * 3 + 1;
        const duration = Math.random() * 8 + 7;
        const delay = Math.random() * -15;
        const drift = Math.round(Math.random() * 100 - 50);
        const brightness = (
            Math.random() * 0.55 + 0.25
        ).toFixed(2);

        ember.style.setProperty(
            "--x",
            `${Math.random() * 100}%`
        );

        ember.style.setProperty("--size", `${size}px`);
        ember.style.setProperty("--duration", `${duration}s`);
        ember.style.setProperty("--delay", `${delay}s`);
        ember.style.setProperty("--drift", `${drift}px`);
        ember.style.setProperty("--brightness", brightness);

        emberLayer.appendChild(ember);

        window.setTimeout(() => {
            ember.remove();
        }, (duration + Math.abs(delay)) * 1000 + 1000);
    }

    if (!reduceMotion && emberLayer) {
        // Esialgsed sädemed.
        for (let i = 0; i < 38; i++) {
            createEmber();
        }

        // Lisa aeg-ajalt uusi sädemeid.
        window.setInterval(() => {
            if (document.hidden) return;

            if (emberLayer.childElementCount < 55) {
                createEmber();
            }
        }, 450);
    }

    // ==========================================
    // 2. KUUM LAAVA, MIS PAISKUB KIRJAST
    // ==========================================

    function emitTextSparks(link, count = 2) {
        if (reduceMotion) return;

        /*
         * Leiame ainult lingi algse tekstisõlme.
         * Nii ei arvutata sädemete asukohta
         * juba loodud sädemete järgi.
         */
        const textNode = Array.from(link.childNodes).find(
            node => node.nodeType === Node.TEXT_NODE
                && node.textContent.trim().length > 0
        );

        if (!textNode) return;

        const range = document.createRange();
        range.selectNodeContents(textNode);

        const textRect = range.getBoundingClientRect();
        const linkRect = link.getBoundingClientRect();

        range.detach();

        if (!textRect.width || !textRect.height) return;

        for (let i = 0; i < count; i++) {
            const spark = document.createElement("span");
            spark.className = "text-spark";
            spark.setAttribute("aria-hidden", "true");

            // Säde algab teksti, mitte nupu serva juurest.
            const x =
                textRect.left - linkRect.left
                + Math.random() * textRect.width;

            const y =
                textRect.top - linkRect.top
                + Math.random() * textRect.height;

            // Iga säde lendab isesuunas.
            const dx = (Math.random() - 0.5) * 70;
            const dy = -(10 + Math.random() * 55);

            const size = 1.5 + Math.random() * 2.5;
            const duration = 450 + Math.random() * 650;

            spark.style.setProperty("--spark-x", `${x}px`);
            spark.style.setProperty("--spark-y", `${y}px`);
            spark.style.setProperty("--spark-size", `${size}px`);
            spark.style.setProperty("--spark-dx", `${dx}px`);
            spark.style.setProperty("--spark-dy", `${dy}px`);

            spark.style.setProperty(
                "--spark-duration",
                `${duration}ms`
            );

            link.appendChild(spark);

            window.setTimeout(() => {
                spark.remove();
            }, duration + 100);
        }
    }

    // ==========================================
    // 3. SÄDEMETE RÜTM IGA MENÜÜKIRJA JUURES
    // ==========================================

    menuLinks.forEach(link => {
        let sparkTimer = null;
        let hovering = false;

        function scheduleSparks() {
            if (reduceMotion || document.hidden) return;

            // Hõljudes tekib rohkem kuumi tilku.
            const count = hovering
                ? 2 + Math.floor(Math.random() * 3)
                : 1 + Math.floor(Math.random() * 2);

            emitTextSparks(link, count);

            const nextDelay = hovering
                ? 220 + Math.random() * 230
                : 650 + Math.random() * 850;

            sparkTimer = window.setTimeout(
                scheduleSparks,
                nextDelay
            );
        }

        function startTextSparks() {
            hovering = true;

            if (sparkTimer !== null) {
                window.clearTimeout(sparkTimer);
            }

            scheduleSparks();
        }

        function stopTextSparks() {
            hovering = false;

            if (sparkTimer !== null) {
                window.clearTimeout(sparkTimer);
            }

            scheduleSparks();
        }

        // Sädemed tekivad ka siis, kui hiir pole lingil.
        scheduleSparks();

        link.addEventListener("mouseenter", startTextSparks);
        link.addEventListener("mouseleave", stopTextSparks);
        link.addEventListener("focus", startTextSparks);
        link.addEventListener("blur", stopTextSparks);
    });

    // ==========================================
    // 4. VAJUTAMISEL LEEGID
    // ==========================================

    function createFlames(link) {
        const burst = document.createElement("span");
        burst.className = "flame-burst";
        burst.setAttribute("aria-hidden", "true");

        const flameCount = 13;

        for (let i = 0; i < flameCount; i++) {
            const flame = document.createElement("span");
            flame.className = "flame";

            const width = Math.random() * 7 + 3;
            const height = Math.random() * 32 + 22;
            const x = Math.random() * 100;
            const drift = Math.round(Math.random() * 34 - 17);
            const angle = Math.round(Math.random() * 36 - 18);
            const duration = Math.random() * 250 + 400;
            const delay = Math.random() * 120;

            flame.style.setProperty("--flame-x", `${x}%`);

            flame.style.setProperty(
                "--flame-width",
                `${width}px`
            );

            flame.style.setProperty(
                "--flame-height",
                `${height}px`
            );

            flame.style.setProperty(
                "--flame-drift",
                `${drift}px`
            );

            flame.style.setProperty(
                "--flame-angle",
                `${angle}deg`
            );

            flame.style.setProperty(
                "--flame-time",
                `${duration}ms`
            );

            flame.style.setProperty(
                "--flame-delay",
                `${delay}ms`
            );

            burst.appendChild(flame);
        }

        link.appendChild(burst);

        window.setTimeout(() => {
            burst.remove();
        }, 1100);
    }

    // ==========================================
    // 5. MENÜÜVAJUTUS JA NAVIGEERIMINE
    // ==========================================

    menuLinks.forEach(link => {
        link.addEventListener("click", event => {
            // Säilita Ctrl-, Shift- ja Meta-klõpsude käitumine.
            if (
                event.ctrlKey ||
                event.metaKey ||
                event.shiftKey ||
                event.altKey ||
                event.button !== 0 ||
                link.target === "_blank"
            ) {
                return;
            }

            if (
                !link.href ||
                link.getAttribute("href") === "#" ||
                link.hasAttribute("download")
            ) {
                return;
            }

            // Kui animatsiooni vähendamine on sisse lülitatud,
            // kasuta tavalist navigeerimist.
            if (reduceMotion) {
                return;
            }

            // Ära ava linki enne leegi animatsiooni lõppu.
            event.preventDefault();

            // Väldi mitut järjestikust vajutust.
            if (link.classList.contains("burning")) {
                return;
            }

            link.classList.add("burning");

            createFlames(link);

            window.setTimeout(() => {
                window.location.assign(link.href);
            }, 700);
        });
    });
});

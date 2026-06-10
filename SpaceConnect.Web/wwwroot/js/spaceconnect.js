(function () {
  'use strict';

  var reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

  /* Page fade-in */
  requestAnimationFrame(function () {
    document.body.classList.add('sc-page-ready');
  });

  /* Hero — image Ken Burns (video removed: bad stock IDs) */
  /* Header solid on scroll (home) */
  var editorialHeader = document.querySelector('.sc-header-editorial');
  if (editorialHeader) {
    var onScroll = function () {
      editorialHeader.classList.toggle('sc-header-scrolled', window.scrollY > 56);
    };
    onScroll();
    window.addEventListener('scroll', onScroll, { passive: true });
  }

  function setCountersFinal() {
    document.querySelectorAll('[data-count]').forEach(function (el) {
      el.textContent = el.getAttribute('data-count');
    });
  }

  function setProgressFinal() {
    document.querySelectorAll('.sc-progress-bar-fill').forEach(function (bar) {
      bar.style.width = (parseInt(bar.getAttribute('data-pct'), 10) || 0) + '%';
    });
  }

  if (reducedMotion) {
    setCountersFinal();
    setProgressFinal();
    document.querySelectorAll('.reveal, .reveal-left').forEach(function (el) {
      el.classList.add('visible');
    });
    return;
  }

  /* Reveal on scroll */
  if ('IntersectionObserver' in window) {
    var revealObs = new IntersectionObserver(function (entries) {
      entries.forEach(function (e) {
        if (!e.isIntersecting) return;
        e.target.classList.add('visible');
        revealObs.unobserve(e.target);

        if (e.target.classList.contains('sc-progress-panel')) {
          animateProgressBars(e.target);
        }
      });
    }, { threshold: 0.1, rootMargin: '0px 0px -24px 0px' });

    document.querySelectorAll('.reveal, .reveal-left, .sc-progress-panel').forEach(function (el) {
      revealObs.observe(el);
    });

    document.querySelectorAll('.sc-dash-metrics, .sc-sector-mosaic, #grid-spinoffs').forEach(function (grid) {
      Array.prototype.forEach.call(grid.children, function (child, i) {
        if (!child.classList.contains('reveal') && !child.classList.contains('reveal-left')) {
          child.classList.add('reveal');
        }
        child.style.transitionDelay = Math.min(i * 0.07, 0.42) + 's';
        revealObs.observe(child);
      });
    });
  }

  function animateCounter(el) {
    var target = parseInt(el.getAttribute('data-count'), 10);
    if (isNaN(target)) return;
    if (target === 0) { el.textContent = '0'; return; }

    var duration = 900;
    var start = performance.now();
    function tick(now) {
      var t = Math.min((now - start) / duration, 1);
      var eased = 1 - Math.pow(1 - t, 3);
      el.textContent = Math.round(target * eased);
      if (t < 1) requestAnimationFrame(tick);
      else el.textContent = target;
    }
    requestAnimationFrame(tick);
  }

  if ('IntersectionObserver' in window) {
    var counterObs = new IntersectionObserver(function (entries) {
      entries.forEach(function (e) {
        if (!e.isIntersecting) return;
        animateCounter(e.target);
        counterObs.unobserve(e.target);
      });
    }, { threshold: 0.5 });

    document.querySelectorAll('[data-count]').forEach(function (el) {
      counterObs.observe(el);
    });
  } else {
    setCountersFinal();
  }

  function animateProgressBars(container) {
    container.querySelectorAll('.sc-progress-bar-fill').forEach(function (bar, i) {
      var pct = parseInt(bar.getAttribute('data-pct'), 10) || 0;
      bar.style.transition = 'width 0.9s cubic-bezier(0.22, 1, 0.36, 1) ' + (i * 0.08) + 's';
      requestAnimationFrame(function () {
        bar.style.width = pct + '%';
      });
    });
  }

  var parallax = document.querySelector('.sc-parallax');
  if (parallax) {
    var inner = parallax.querySelector('.sc-parallax-inner');
    var strength = parseFloat(parallax.getAttribute('data-parallax')) || 0.2;
    if (inner) {
      window.addEventListener('scroll', function () {
        var rect = parallax.getBoundingClientRect();
        if (rect.bottom < 0 || rect.top > window.innerHeight) return;
        var offset = (rect.top - window.innerHeight * 0.35) * strength;
        inner.style.transform = 'translate3d(0, ' + offset + 'px, 0)';
      }, { passive: true });
    }
  }
})();

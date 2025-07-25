$(document).ready(function() {
    // Back to top functionality with smooth animation
    $(window).scroll(function() {
        if ($(this).scrollTop() > 200) {
            $('#backToTop').addClass('visible');
        } else {
            $('#backToTop').removeClass('visible');
        }
    });

    $('#backToTop').click(function() {
        $('html, body').animate({scrollTop: 0}, 800, 'easeOutCubic');
        return false;
    });

    // Enhanced navbar scroll effect
    $(window).scroll(function() {
        if ($(this).scrollTop() > 50) {
            $('.navbar-glass').addClass('scrolled');
        } else {
            $('.navbar-glass').removeClass('scrolled');
        }
    });

    // Animate floating cards with staggered delays
    function animateCards() {
        $('.floating-card').each(function(index) {
            $(this).css({
                'animation-delay': (index * 0.3) + 's',
                'animation-fill-mode': 'both'
            });
        });
    }

    // Stats counter animation
    function animateStats() {
        $('.stat-number').each(function() {
            const $this = $(this);
            const finalNumber = $this.text();
            $this.prop('Counter', 0).animate({
                Counter: parseInt(finalNumber.replace(/[^\d]/g, ''))
            }, {
                duration: 2000,
                easing: 'swing',
                step: function(now) {
                    if (finalNumber.includes('%')) {
                        $this.text(Math.ceil(now) + '%');
                    } else if (finalNumber.includes('+')) {
                        $this.text(Math.ceil(now) + '+');
                    } else if (finalNumber.includes('/')) {
                        $this.text(finalNumber);
                    } else {
                        $this.text(Math.ceil(now));
                    }
                }
            });
        });
    }

    // Initialize animations
    animateCards();

    // Trigger stats animation when hero section is in view
    const heroSection = $('.hero-section');
    let statsAnimated = false;

    $(window).scroll(function() {
        if (!statsAnimated && heroSection.offset().top < $(window).scrollTop() + $(window).height()) {
            animateStats();
            statsAnimated = true;
        }
    });

    // Smooth scrolling for anchor links
    $('a[href^="#"]').on('click', function(event) {
        const target = $(this.getAttribute('href'));
        if (target.length) {
            event.preventDefault();
            $('html, body').stop().animate({
                scrollTop: target.offset().top - 100
            }, 800);
        }
    });

    // Add loading animation to navbar brand
    $('.navbar-brand').hover(
        function() { $(this).addClass('pulse-animation'); },
        function() { $(this).removeClass('pulse-animation'); }
    );
});

// Parallax effect for hero shapes
$(window).scroll(function() {
    const scrolled = $(this).scrollTop();
    const rate = scrolled * -0.5;
    const rate2 = scrolled * -0.3;

    $('.hero-shape-1').css('transform', 'translateY(' + rate + 'px)');
    $('.hero-shape-2').css('transform', 'translateY(' + rate2 + 'px)');
});
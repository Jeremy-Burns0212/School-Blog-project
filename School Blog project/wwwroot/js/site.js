// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Apply site colors immediately when the admin settings form is submitted.
document.addEventListener('DOMContentLoaded', function () {
	function normalizeHex(value) {
		const v = (value || '').trim();
		if (!v) return '';
		return v.startsWith('#') ? v.toUpperCase() : ('#' + v.toUpperCase());
	}

	function inverseHexColor(hex) {
		const cleaned = normalizeHex(hex).replace('#', '');
		if (!/^([0-9A-F]{6})$/.test(cleaned)) {
			return '#000000';
		}

		const r = 255 - parseInt(cleaned.slice(0, 2), 16);
		const g = 255 - parseInt(cleaned.slice(2, 4), 16);
		const b = 255 - parseInt(cleaned.slice(4, 6), 16);
		return '#' + [r, g, b].map(value => value.toString(16).padStart(2, '0')).join('').toUpperCase();
	}

	function applyColors(primary, secondary) {
		if (primary) document.documentElement.style.setProperty('--site-primary', primary);
		if (secondary) document.documentElement.style.setProperty('--site-secondary', secondary);
		try {
			// also apply inline backgrounds to ensure immediate effect
			if (primary) {
				const nav = document.querySelector('.site-navbar');
				if (nav) nav.style.background = primary;
				const navChildren = document.querySelectorAll('.site-navbar .navbar-collapse, .site-navbar .navbar-toggler, .site-navbar .navbar-brand, .site-navbar .form-control');
				navChildren.forEach(el => el.style.background = primary);
			}
			if (secondary) {
				document.body.style.background = secondary;
				const main = document.querySelector('main[role="main"]');
				if (main) main.style.background = secondary;
				const footerAccent = document.querySelector('.site-footer-accent');
				if (footerAccent) footerAccent.style.background = secondary;
			}
				// compute and apply inverse text color for navbar text elements
				if (primary) {
					const inverseColor = inverseHexColor(primary);

					try {
						const navTextEls = document.querySelectorAll('.site-navbar .navbar-brand, .site-navbar .nav-link, .site-navbar .navbar-text, .site-navbar .dropdown-toggle, .site-navbar .btn');
						navTextEls.forEach(el => el.style.setProperty('color', inverseColor, 'important'));
					} catch (err) { /* ignore */ }
				}
		} catch (err) {
			console.error('Failed to apply inline backgrounds', err);
		}
	}

	document.addEventListener('submit', function (e) {
		try {
			const form = e.target;
			if (!form || !(form instanceof HTMLFormElement)) return;
			// Only react to the settings form which contains the color pickers
			const primaryPicker = form.querySelector('#SettingsForm_PrimaryColorPicker');
			const primaryText = form.querySelector('#SettingsForm_PrimaryColor');
			const secondaryPicker = form.querySelector('#SettingsForm_SecondaryColorPicker');
			const secondaryText = form.querySelector('#SettingsForm_SecondaryColor');
			if (!primaryPicker && !primaryText && !secondaryPicker && !secondaryText) return;

						const p = normalizeHex((primaryText && primaryText.value) || (primaryPicker && primaryPicker.value) || '');
						const s = normalizeHex((secondaryText && secondaryText.value) || (secondaryPicker && secondaryPicker.value) || '');
						try {
							if (p) sessionStorage.setItem('pendingSitePrimary', p);
							if (s) sessionStorage.setItem('pendingSiteSecondary', s);
						} catch (err) { }
						applyColors(p, s);
			// allow form to submit normally
		} catch (err) {
			console.error('Error applying site colors on submit', err);
		}
		}, true);

		// On load apply any pending values saved before submit (to survive round-trip)
		try {
			const pendingP = sessionStorage.getItem('pendingSitePrimary');
			const pendingS = sessionStorage.getItem('pendingSiteSecondary');
			if (pendingP || pendingS) {
				applyColors(pendingP || '', pendingS || '');
				// clear pending after a short delay
				setTimeout(() => {
					try { sessionStorage.removeItem('pendingSitePrimary'); sessionStorage.removeItem('pendingSiteSecondary'); } catch (err) { }
				}, 3000);
			}
		} catch (err) { }
});

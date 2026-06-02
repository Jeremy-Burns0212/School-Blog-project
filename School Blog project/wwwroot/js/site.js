// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

(function () {
	function normalizeHex(value) {
		const trimmed = (value || '').trim();
		if (!trimmed) {
			return '';
		}

		return trimmed.startsWith('#') ? trimmed.toUpperCase() : `#${trimmed.toUpperCase()}`;
	}

	function saveColors(primary, secondary, tertiary) {
		try {
			if (primary) {
				localStorage.setItem('primaryColor', primary);
			}

			if (secondary) {
				localStorage.setItem('secondaryColor', secondary);
			}

			if (tertiary) {
				localStorage.setItem('tertiaryColor', tertiary);
			}
		} catch (error) {
			console.warn('Unable to save site colors.', error);
		}
	}

	function applySavedColors() {
		if (typeof window.applySavedColors === 'function') {
			window.applySavedColors();
		}
	}

	function readColor(form, pickerId, textId) {
		const picker = form.querySelector(`#${pickerId}`);
		const text = form.querySelector(`#${textId}`);
		return normalizeHex((text && text.value) || (picker && picker.value) || '');
	}

	document.addEventListener('DOMContentLoaded', function () {
		applySavedColors();

		document.addEventListener('submit', function (event) {
			const form = event.target;
			if (!form || !(form instanceof HTMLFormElement)) {
				return;
			}

			const hasPrimary = form.querySelector('#SettingsForm_PrimaryColorPicker') || form.querySelector('#SettingsForm_PrimaryColor');
			const hasSecondary = form.querySelector('#SettingsForm_SecondaryColorPicker') || form.querySelector('#SettingsForm_SecondaryColor');
			const hasTertiary = form.querySelector('#SettingsForm_TertiaryColorPicker') || form.querySelector('#SettingsForm_TertiaryColor');
			if (!hasPrimary && !hasSecondary && !hasTertiary) {
				return;
			}

			const primary = readColor(form, 'SettingsForm_PrimaryColorPicker', 'SettingsForm_PrimaryColor');
			const secondary = readColor(form, 'SettingsForm_SecondaryColorPicker', 'SettingsForm_SecondaryColor');
			const tertiary = readColor(form, 'SettingsForm_TertiaryColorPicker', 'SettingsForm_TertiaryColor');
			saveColors(primary, secondary, tertiary);
			applySavedColors();
		}, true);
	});
})();

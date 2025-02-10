/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Views/**/*.cshtml', // Razor View
    './wwwroot/js/**/*.js' // Statik JavaScript
  ],
  theme: {
    extend: {
      colors: {
        appPrimary: '#F6792C',
        appSecondary: '#008037',
        appWarning: '#ED1212',
        appCartBg: '#FEFEFE',
        appStroke: '#939393',
        appText: '#4E4E4E',
        appBgMain: '#F5F5F5',
        appBgSecondary: '#FDFDFD',
        appDisabledBtn: '#FFCEB1',
        appOverlay: '#D9D9D9'
      },
      fontFamily: {
        sans: ['Quicksand', 'sans-serif'], // Quicksand varsayilan
        inter: ['Inter', 'serif'] // Inter
      }
    },
  },
  plugins: [
    require('tailwind-scrollbar-hide'),
  ],
}


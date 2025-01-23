/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Views/**/*.cshtml', // Razor View
    './wwwroot/js/**/*.js' // Statik JavaScript
  ],
  theme: {
    extend: {
      colors: {
        primary: '#F6792C',
        secondary: '#008037',
        warning: '#ED1212',
        cartBg: '#FEFEFE',
        stroke: '#939393',
        text: '#4E4E4E',
        bgMain: '#F5F5F5'
      },
      fontFamily: {
        sans: ['Quicksand', 'sans-serif'] // Quicksand varsayılan sans-serif fontu
      }
    },
  },
  plugins: [],
}


/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.ts", "./src/**/*.tsx", "./src/**/*.css","./src/*.tsx","./src/*.ts","./src/*.css"],
  theme: {
    extend: {},
    colors: {
      'text': 'var(--text)',
      'background': 'var(--background)',
      'primary': 'var(--primary)',
      'secondary': 'var(--secondary)',
      'accent': 'var(--accent)',
      'green': 'var(--green)',
      'red': 'var(--red)',
      'gold': 'var(--gold)',
      'bronce': 'var(--bronce)',
      'silver': 'var(--silver)',
    },
  },
  plugins: [],
}


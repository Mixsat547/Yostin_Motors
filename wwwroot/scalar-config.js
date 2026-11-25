// Hace que Scalar envíe la cookie de sesión en cada petición
const originalFetch = window.fetch;
window.fetch = (input, init = {}) =>
  originalFetch(input, { ...init, credentials: 'same-origin' });
document.addEventListener('click', function (event) {
  var button = event.target.closest('[data-password-toggle]');
  if (!button) return;
  var input = document.getElementById(button.getAttribute('data-password-toggle'));
  if (!input) return;
  var hidden = input.type === 'password';
  input.type = hidden ? 'text' : 'password';
  button.textContent = hidden ? 'Ocultar' : 'Mostrar';
});

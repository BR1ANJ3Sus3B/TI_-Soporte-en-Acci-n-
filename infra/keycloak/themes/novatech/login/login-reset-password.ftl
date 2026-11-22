<#import "template.ftl" as layout>
<@layout.registrationLayout displayMessage=!messagesPerField.existsError('username'); section>
  <#if section = "header"><div class="novatech-brand"><span class="novatech-mark">N</span><span><strong>TI: Soporte en Acción</strong><small>NovaTech Solutions</small></span></div>
  <#elseif section = "form"><h1 class="novatech-title">Recupera tu contraseña</h1><p class="novatech-intro">Escribe tu correo y te enviaremos instrucciones si SMTP está configurado.</p><form id="kc-reset-password-form" class="novatech-form" action="${url.loginAction}" method="post"><div class="form-group"><label for="username">Correo electrónico</label><div class="novatech-input"><span>@</span><input type="email" id="username" name="username" autofocus autocomplete="email" required /></div><#if messagesPerField.existsError('username')><span class="kc-feedback-text">${kcSanitize(messagesPerField.getFirstError('username'))?no_esc}</span></#if></div><button class="novatech-submit" type="submit">ENVIAR INSTRUCCIONES</button></form><p class="novatech-footer"><a href="${url.loginUrl}">Volver a iniciar sesión</a></p>
  </#if>
</@layout.registrationLayout>

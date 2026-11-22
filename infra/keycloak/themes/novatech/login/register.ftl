<#import "template.ftl" as layout>
<@layout.registrationLayout displayMessage=!messagesPerField.existsError('firstName','lastName','email','username','password','password-confirm'); section>
  <#if section = "header">
    <div class="novatech-brand"><span class="novatech-mark" aria-hidden="true">N</span><span><strong>TI: Soporte en Acción</strong><small>NovaTech Solutions</small></span></div>
  <#elseif section = "form">
    <h1 class="novatech-title">Crea tu cuenta</h1><p class="novatech-intro">Comienza tu aventura tecnológica.</p>
    <form id="kc-register-form" class="novatech-form" action="${url.registrationAction}" method="post">
      <div class="novatech-grid">
        <div class="form-group"><label for="firstName">Nombre(s)</label><div class="novatech-input"><span>◉</span><input type="text" id="firstName" name="firstName" value="${(register.formData.firstName!'')}" autocomplete="given-name" autofocus required /></div><#if messagesPerField.existsError('firstName')><span class="kc-feedback-text">${kcSanitize(messagesPerField.getFirstError('firstName'))?no_esc}</span></#if></div>
        <div class="form-group"><label for="lastName">Apellidos</label><div class="novatech-input"><span>◉</span><input type="text" id="lastName" name="lastName" value="${(register.formData.lastName!'')}" autocomplete="family-name" required /></div><#if messagesPerField.existsError('lastName')><span class="kc-feedback-text">${kcSanitize(messagesPerField.getFirstError('lastName'))?no_esc}</span></#if></div>
      </div>
      <div class="form-group"><label for="email">Correo electrónico</label><div class="novatech-input"><span>@</span><input type="email" id="email" name="email" value="${(register.formData.email!'')}" autocomplete="email" required /></div><#if messagesPerField.existsError('email')><span class="kc-feedback-text">${kcSanitize(messagesPerField.getFirstError('email'))?no_esc}</span></#if></div>
      <#if !realm.registrationEmailAsUsername><div class="form-group"><label for="username">Usuario</label><input type="text" id="username" name="username" value="${(register.formData.username!'')}" autocomplete="username" required /></div></#if>
      <div class="form-group"><label for="password">Contraseña</label><div class="novatech-password novatech-input"><span>◈</span><input type="password" id="password" name="password" autocomplete="new-password" required /><button type="button" aria-label="Mostrar contraseña" data-password-toggle="password">Mostrar</button></div><#if messagesPerField.existsError('password')><span class="kc-feedback-text">${kcSanitize(messagesPerField.getFirstError('password'))?no_esc}</span></#if></div>
      <div class="form-group"><label for="password-confirm">Confirmar contraseña</label><div class="novatech-password novatech-input"><span>◈</span><input type="password" id="password-confirm" name="password-confirm" autocomplete="new-password" required /><button type="button" aria-label="Mostrar contraseña" data-password-toggle="password-confirm">Mostrar</button></div><#if messagesPerField.existsError('password-confirm')><span class="kc-feedback-text">${kcSanitize(messagesPerField.getFirstError('password-confirm'))?no_esc}</span></#if></div>
      <#if recaptchaRequired??><div class="form-group"><div class="g-recaptcha" data-size="compact" data-sitekey="${recaptchaSiteKey}"></div></div></#if>
      <button class="novatech-submit" type="submit">CREAR CUENTA</button>
    </form>
    <div class="novatech-divider"><span>o</span></div>
    <#if social.providers?? && social.providers?has_content><div class="novatech-social"><#list social.providers as p><a class="novatech-google" href="${p.loginUrl}">Registrarme con ${p.displayName}</a></#list></div></#if>
    <p class="novatech-footer">¿Ya tienes una cuenta? <a href="${url.loginUrl}">Inicia sesión</a></p>
  </#if>
</@layout.registrationLayout>

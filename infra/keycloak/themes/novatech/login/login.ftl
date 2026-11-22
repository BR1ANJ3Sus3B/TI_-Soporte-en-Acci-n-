<#import "template.ftl" as layout>
<@layout.registrationLayout displayMessage=!messagesPerField.existsError('username','password') displayInfo=realm.password && realm.registrationAllowed && !registrationDisabled??; section>
  <#if section = "header">
    <div class="novatech-brand" aria-label="NovaTech Solutions">
      <span class="novatech-mark" aria-hidden="true">N</span>
      <span><strong>TI: Soporte en Acción</strong><small>NovaTech Solutions</small></span>
    </div>
  <#elseif section = "form">
    <div class="novatech-intro">Accede con tu cuenta NovaTech para sincronizar tu progreso de forma segura.</div>
    <form id="kc-form-login" class="novatech-form" onsubmit="login.disabled = true; return true;" action="${url.loginAction}" method="post">
      <div class="form-group">
        <label for="username">Correo electrónico</label>
        <input tabindex="1" id="username" class="${properties.kcInputClass!}" name="username" value="${(login.username!'')}" type="text" autofocus autocomplete="username" aria-invalid="${messagesPerField.existsError('username')?c}" />
        <#if messagesPerField.existsError('username')><span class="kc-feedback-text">${kcSanitize(messagesPerField.getFirstError('username'))?no_esc}</span></#if>
      </div>
      <div class="form-group">
        <label for="password">Contraseña</label>
        <div class="novatech-password"><input tabindex="2" id="password" class="${properties.kcInputClass!}" name="password" type="password" autocomplete="current-password" aria-invalid="${messagesPerField.existsError('password')?c}" /><button type="button" aria-label="Mostrar contraseña" onclick="var p=document.getElementById('password');p.type=p.type==='password'?'text':'password';this.classList.toggle('visible');">◉</button></div>
        <#if messagesPerField.existsError('password')><span class="kc-feedback-text">${kcSanitize(messagesPerField.getFirstError('password'))?no_esc}</span></#if>
      </div>
      <div class="novatech-options">
        <#if realm.rememberMe && !usernameEditDisabled??><label><input tabindex="3" id="rememberMe" name="rememberMe" type="checkbox" <#if login.rememberMe??>checked</#if> /> Recordarme</label></#if>
        <#if realm.resetPasswordAllowed><a tabindex="5" href="${url.loginResetCredentialsUrl}">¿Olvidaste tu contraseña?</a></#if>
      </div>
      <button tabindex="4" class="novatech-submit" name="login" id="kc-login" type="submit">Iniciar sesión</button>
    </form>
  <#elseif section = "info">
    <#if realm.password && realm.registrationAllowed && !registrationDisabled??><p>¿Aún no tienes cuenta? <a href="${url.registrationUrl}">Crear una cuenta gratis</a></p></#if>
  </#if>
</@layout.registrationLayout>

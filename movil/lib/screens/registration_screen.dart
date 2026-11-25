import 'package:flutter/material.dart';
import 'package:ti_soporte_accion/services/api_client.dart';
import 'package:ti_soporte_accion/services/auth_service.dart';
import 'package:ti_soporte_accion/services/registration_service.dart';
import 'package:ti_soporte_accion/theme/app_colors.dart';
import 'package:ti_soporte_accion/theme/app_typography.dart';
import 'package:ti_soporte_accion/widgets/nova_panel.dart';

class RegistrationScreen extends StatefulWidget {
  const RegistrationScreen({super.key, this.service});
  static const routeName = '/register';
  final RegistrationService? service;
  @override
  State<RegistrationScreen> createState() => _RegistrationScreenState();
}

class _RegistrationScreenState extends State<RegistrationScreen> {
  final _form = GlobalKey<FormState>();
  final _name = TextEditingController();
  final _email = TextEditingController();
  final _password = TextEditingController();
  final _confirm = TextEditingController();
  late final _service = widget.service ?? RegistrationService();
  bool _hidden = true, _busy = false, _submitted = false;
  String? _error;
  DateTime? _resendAfter;

  Future<void> _submit() async {
    if (_busy || !_form.currentState!.validate()) return;
    setState(() {
      _busy = true;
      _error = null;
    });
    try {
      await _service.register(_name.text, _email.text, _password.text);
      if (!mounted) return;
      _password.clear();
      _confirm.clear();
      if (mounted) {
        setState(() {
          _submitted = true;
          _resendAfter = DateTime.now().add(const Duration(seconds: 60));
        });
      }
    } catch (error) {
      if (mounted) {
        setState(
          () => _error = error is ApiException
              ? error.message
              : 'No se pudo completar el registro. Inténtalo más tarde.',
        );
      }
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  Future<void> _resend() async {
    if (_busy) return;
    if (_resendAfter != null && DateTime.now().isBefore(_resendAfter!)) {
      setState(
        () =>
            _error = 'Espera un minuto antes de volver a solicitar el correo.',
      );
      return;
    }
    setState(() {
      _busy = true;
      _error = null;
    });
    try {
      await _service.resend(_email.text);
      _resendAfter = DateTime.now().add(const Duration(seconds: 60));
      if (mounted) {
        setState(
          () => _error =
              'Solicitud recibida. Revisa tu correo y la carpeta de spam.',
        );
      }
    } catch (error) {
      if (mounted) {
        setState(
          () => _error = error is ApiException
              ? error.message
              : 'No se pudo solicitar el correo.',
        );
      }
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  Future<void> _login() async {
    if (_busy) return;
    setState(() {
      _busy = true;
      _error = null;
    });
    try {
      if (await AuthService.instance.login() && mounted) {
        Navigator.of(context).pushNamedAndRemoveUntil('/home', (_) => false);
      }
    } catch (_) {
      if (mounted) {
        setState(
          () => _error = 'No fue posible iniciar sesión. Verifica tu correo e inténtalo de nuevo.',
        );
      }
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  @override
  void dispose() {
    _name.dispose();
    _email.dispose();
    _password.dispose();
    _confirm.dispose();
    if (widget.service == null) _service.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => Scaffold(
    appBar: AppBar(title: const Text('Registro')),
    body: Container(
      decoration: const BoxDecoration(gradient: AppColors.backgroundGradient),
      child: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(24),
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 460),
              child: NovaPanel(
                child: Form(
                  key: _form,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.stretch,
                    children: [
                      Image.asset(
                        'assets/icons/Soporte en Acción Pixelado.png',
                        height: 96,
                        semanticLabel: 'TI: Soporte en Acción',
                      ),
                      Text(
                        'NOVATECH SOLUTIONS',
                        textAlign: TextAlign.center,
                        style: AppTypography.label(12, color: AppColors.cyan),
                      ),
                      const SizedBox(height: 20),
                      Text(
                        _submitted ? 'Revisa tu correo' : 'Crea tu cuenta',
                        style: AppTypography.heading(28),
                      ),
                      Text(
                        _submitted
                            ? 'Solicitud recibida. Si la cuenta puede recibir verificación, encontrarás las instrucciones en tu correo. Si ya tienes cuenta, inicia sesión o recupera el acceso en Keycloak.'
                            : 'Comienza tu aventura tecnológica',
                        style: AppTypography.body(
                          15,
                          color: AppColors.textSecondary,
                        ),
                      ),
                      const SizedBox(height: 20),
                      if (!_submitted) ...[
                        TextFormField(
                          controller: _name,
                          enabled: !_busy,
                          textCapitalization: TextCapitalization.words,
                          decoration: const InputDecoration(
                            labelText: 'Nombre completo',
                          ),
                          maxLength: 100,
                          validator: (v) => (v?.trim().length ?? 0) < 2
                              ? 'Introduce tu nombre completo.'
                              : null,
                        ),
                        const SizedBox(height: 12),
                        TextFormField(
                          controller: _email,
                          enabled: !_busy,
                          keyboardType: TextInputType.emailAddress,
                          autocorrect: false,
                          decoration: const InputDecoration(
                            labelText: 'Correo electrónico',
                          ),
                          validator: (v) =>
                              RegistrationService.validEmail(v ?? '')
                              ? null
                              : 'Introduce un correo válido.',
                        ),
                        const SizedBox(height: 12),
                        TextFormField(
                          controller: _password,
                          enabled: !_busy,
                          obscureText: _hidden,
                          autocorrect: false,
                          enableSuggestions: false,
                          onChanged: (_) => setState(() {}),
                          decoration: InputDecoration(
                            labelText: 'Contraseña',
                            suffixIcon: IconButton(
                              tooltip: _hidden
                                  ? 'Mostrar contraseña'
                                  : 'Ocultar contraseña',
                              onPressed: () =>
                                  setState(() => _hidden = !_hidden),
                              icon: Icon(
                                _hidden
                                    ? Icons.visibility
                                    : Icons.visibility_off,
                              ),
                            ),
                          ),
                          validator: (v) =>
                              RegistrationService.validPassword(v ?? '')
                              ? null
                              : 'La contraseña no cumple los requisitos.',
                        ),
                        const SizedBox(height: 8),
                        Text(
                          '${RegistrationService.validPassword(_password.text) ? '✓' : '○'} 12–128 caracteres, mayúscula, minúscula, número y símbolo.',
                          style: AppTypography.body(
                            12,
                            color:
                                RegistrationService.validPassword(
                                  _password.text,
                                )
                                ? AppColors.cyan
                                : AppColors.textSecondary,
                          ),
                        ),
                        const SizedBox(height: 12),
                        TextFormField(
                          controller: _confirm,
                          enabled: !_busy,
                          obscureText: _hidden,
                          autocorrect: false,
                          enableSuggestions: false,
                          decoration: const InputDecoration(
                            labelText: 'Confirmar contraseña',
                          ),
                          validator: (v) =>
                              v == _password.text && (v?.isNotEmpty ?? false)
                              ? null
                              : 'Las contraseñas no coinciden.',
                        ),
                        const SizedBox(height: 20),
                        FilledButton(
                          onPressed: _busy ? null : _submit,
                          child: const Text('Crear cuenta'),
                        ),
                      ] else ...[
                        const Text(
                          'Verifica tu correo antes de iniciar sesión. Si el enlace expiró, solicita uno nuevo.',
                        ),
                        TextButton(
                          onPressed: _busy ? null : _resend,
                          child: const Text('Reenviar verificación'),
                        ),
                      ],
                      TextButton(
                        onPressed: _busy || !AuthService.instance.isConfigured
                            ? null
                            : _login,
                        child: const Text('Iniciar sesión'),
                      ),
                      if (_busy) const LinearProgressIndicator(),
                      if (_error != null)
                        Padding(
                          padding: const EdgeInsets.only(top: 12),
                          child: Text(
                            _error!,
                            style: AppTypography.body(
                              14,
                              color: AppColors.warning,
                            ),
                          ),
                        ),
                    ],
                  ),
                ),
              ),
            ),
          ),
        ),
      ),
    ),
  );
}

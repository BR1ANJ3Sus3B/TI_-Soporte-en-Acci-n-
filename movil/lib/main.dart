import 'package:flutter/material.dart';

import 'core/telemetry/telemetry_service.dart';

void main() {
  final telemetry = TelemetryService(
    // Para el emulador Android, 10.0.2.2 apunta al localhost de la maquina.
    baseUri: Uri.parse(const String.fromEnvironment(
      'API_URL',
      defaultValue: 'http://10.0.2.2:4000',
    )),
  );
  telemetry.track('app.started').catchError((_) {});
  runApp(TelemetryDemo(telemetry: telemetry));
}

class TelemetryDemo extends StatelessWidget {
  const TelemetryDemo({super.key, required this.telemetry});

  final TelemetryService telemetry;

  @override
  Widget build(BuildContext context) => MaterialApp(
        home: Scaffold(
          appBar: AppBar(title: const Text('TI: Soporte en Accion')),
          body: Center(
            child: ElevatedButton(
              onPressed: () => telemetry.track('mission.opened'),
              child: const Text('Registrar apertura de mision'),
            ),
          ),
        ),
      );
}

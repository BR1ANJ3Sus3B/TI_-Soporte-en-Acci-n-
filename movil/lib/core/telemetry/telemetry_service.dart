import 'dart:convert';
import 'dart:io';

/// Envia eventos agregados a la API. Nunca envia identificadores personales.
class TelemetryService {
  TelemetryService({required Uri baseUri, HttpClient? httpClient})
      : _baseUri = baseUri,
        _httpClient = httpClient ?? HttpClient();

  final Uri _baseUri;
  final HttpClient _httpClient;

  Future<void> track(String name, {Duration? duration}) async {
    final request = await _httpClient.postUrl(
      _baseUri.resolve('/api/telemetry/events'),
    );
    request.headers.contentType = ContentType.json;
    request.write(jsonEncode({
      'name': name,
      'platform': 'flutter',
      if (duration != null) 'durationMs': duration.inMilliseconds,
    }));

    final response = await request.close();
    if (response.statusCode < 200 || response.statusCode >= 300) {
      throw HttpException('La telemetria no fue aceptada: ${response.statusCode}');
    }
    await response.drain();
  }

  void dispose() => _httpClient.close();
}

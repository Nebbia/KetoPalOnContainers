{{- define "suffix-name" -}}
{{- if .Values.app.name -}}
{{- .Values.app.name -}}
{{- else -}}
{{- .Release.Name -}}
{{- end -}}
{{- end -}}

{{- define "keystore-constr" -}}
{{- if index .Values "keystore-data" "service" "name" -}}
{{- $constr := index .Values "keystore-data" "service" "name" -}}}}
{{- printf "%s:6379" $constr -}}
{{- else -}}
{{- .Values.inf.redis.keystore.constr -}}
{{- end -}}
{{- end -}}

{{- define "pathBase" -}}
{{- if .Values.inf.k8s.suffix -}}
{{- $suffix := include "suffix-name" . -}}
{{- printf "%s-%s"  .Values.pathBase $suffix -}}
{{- else -}}
{{- .Values.pathBase -}}
{{- end -}}
{{- end -}}

{{- define "fqdn-image" -}}
{{- if .Values.inf.registry -}}
{{- printf "%s/%s" .Values.inf.registry.server .Values.image.repository -}}
{{- else -}}
{{- .Values.image.repository -}}
{{- end -}}
{{- end -}}
# Followed instructions from https://docs.microsoft.com/en-us/azure/aks/ingress-static-ip

$aksResourceGroup = az aks show --resource-group prod-nebbia-rg --name prod-nebbia-aks --query nodeResourceGroup -o tsv

$publicIp = az network public-ip create -g $aksResourceGroup -n prod-ketopal-api-ip --allocation-method static --query publicIp.ipAddress -o tsv

kubectl create namespace ketopal-ingress

helm install stable/nginx-ingress `
    --namespace ketopal-ingress `
    --set controller.replicaCount=3 `
    --set controller.nodeSelector."beta\.kubernetes\.io/os"=linux `
    --set defaultBackend.nodeSelector."beta\.kubernetes\.io/os"=linux `
    --set controller.service.loadBalancerIP="$publicIp" `
    --set controller.service.externalTrafficPolicy=Local


# verify that the load balancer gets created by (verify it has an external ip address)
kubectl get service -l app=nginx-ingress --namespace ketopal-ingress

# Public IP address of your ingress controller (external ip address)
$IP=$publicIp

# Name to associate with public IP address
$DNSNAME="keto-pal-api"

# Get the resource-id of the public ip
$PUBLICIPID=$(az network public-ip list --query "[?ipAddress!=null]|[?contains(ipAddress, '$IP')].[id]" --output tsv)

# Update public ip address with DNS name
az network public-ip update --ids $PUBLICIPID --dns-name $DNSNAME

# Install cert-manager

# Install the CustomResourceDefinition resources separately
kubectl apply -f https://raw.githubusercontent.com/jetstack/cert-manager/release-0.8/deploy/manifests/00-crds.yaml

# Create the namespace for cert-manager
kubectl create namespace cert-manager

# Label the cert-manager namespace to disable resource validation
kubectl label namespace cert-manager certmanager.k8s.io/disable-validation=true

# Add the Jetstack Helm repository
helm repo add jetstack https://charts.jetstack.io

# Update your local Helm chart repository cache
helm repo update

# Install the cert-manager Helm chart
helm install `
  --name cert-manager `
  --namespace cert-manager `
  --version v0.8.0 `
  jetstack/cert-manager


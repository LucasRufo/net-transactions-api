kubectl config use-context kind-kind

kubectl create namespace net-transactions

helm uninstall net-transactions -n net-transactions

dotnet cake --target=BuildApiImage

dotnet cake --target=ExecuteMigrations

kubectl label namespace net-transactions istio-injection=enabled --overwrite

helm install net-transactions ../chart/net-transactions/ --debug --wait `
    -n net-transactions `
    -f ../chart/net-transactions/values.local.yaml

    

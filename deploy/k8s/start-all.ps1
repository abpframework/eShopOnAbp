kubectl apply `
    -f ./namespace.yaml `
    -f ./rabbitmq.yaml `
	-f ./rabbitmq-admin.yaml `
    -f ./redis.yaml `
	-f ./sqlserver.yaml `
    -f ./sql-service.yaml
# nuages-taskqueue

**What is nuages-taskqueue?**

nuages-taskqueue aims at providing a easy way to enqueue/dequeue/execute tasks form a persistent queue such as [AWS Simple Storage Queue (SQS)](https://aws.amazon.com/sqs/) or [Azure Storage Queue (ASQ)](https://docs.microsoft.com/en-us/azure/storage/queues/storage-queues-introduction).


### Nuages TaskRunner

Nuages.TaskRunner is package that provides functionalities to execute code based on a generic serialized Json representation.

[Nuages.TaskRunner](https://www.nuget.org/packages/Nuages.TaskRunner/) is available as a nuget package.,

### Nuages.Queue

Nuages.Queue introduce the QueryWorker abstract class which is responsible to get message from a queue. The class needs to inherited from in order to provide overload for queue manipulations.

Two pre-build package are available on nuget :

- [Nuages.Queue.SQS](https://www.nuget.org/packages/Nuages.Queue.SQS/) for Simple Queue Service (SQS) on AWS
- [Nuages.Queue.ASQ](https://www.nuget.org/packages/Nuages.Queue.ASQ/) for Azure Storage Queue on Azure

### Nuages.TaskQueue

Nuages.TaskQueue bring Nuages.TaskRunner and Nuages.Queue together to offer a hosted task running service based on message from a queue.

It is available in two packages :

- Nuages.TaskQueue.SQS
- Nuages.TaskQueue.ASQ

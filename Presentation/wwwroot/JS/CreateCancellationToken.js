function CreateCancellationToken() {
    const controller = new AbortController();
    const signal = controller.signal;
    setTimeout(() => controller.abort(), 2000);
    return signal;
}
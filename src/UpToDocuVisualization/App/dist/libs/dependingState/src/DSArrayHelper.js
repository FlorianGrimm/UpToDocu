export function dsIsArrayEqual(arrOld, arrNew, fnIsEqual) {
    if (arrOld.length !== arrNew.length) {
        return false;
    }
    for (let idx = 0; idx < arrNew.length; idx++) {
        if (!fnIsEqual(arrOld[idx], arrNew[idx])) {
            return false;
        }
    }
    return true;
}

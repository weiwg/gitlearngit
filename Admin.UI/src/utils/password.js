import md5 from 'js-md5';

export const passwordMd5=(val)=>{
    return md5(val+md5(val).toUpperCase()+val).toUpperCase();
}
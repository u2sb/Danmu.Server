import { decodeAsync } from "https://cdn.jsdelivr.net/npm/@msgpack/msgpack/+esm";

const getDanMuAsync = async (url) => {
  const mb = await fetch(url, {
    headers: { "Content-Type": "application/x-msgpack" },
  });
  const md = await decodeAsync(mb.body);

  return md.map((item) => ({
    text: item[0],
    model: item[1],
    color: item[2],
    time: item[3],
    border: item[4],
    style: item[5],
  }));
};

export { getDanMuAsync };

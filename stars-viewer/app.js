const fs = require('fs')
const path = require('path')
const express = require('express')
const app = express()

const tempFile = path.join(__dirname, "./public/data/stars.temp")

app.use(express.json())

app.get('', (req, res) => {
  res.sendFile(path.join(__dirname, "./public/index.html"))
})
app.get('/export', (req, res) => {
  var d3 = fs.readFileSync(path.join(__dirname, "./public/js/d3.min.js"))
  var geo = fs.readFileSync(path.join(__dirname, "./public/js/d3.geo.projection.min.js"))
  var css = fs.readFileSync(path.join(__dirname, "./public/css/celestial.css"))
  var stars = fs.readFileSync(path.join(__dirname, "./public/data/stars.json")).toString()
  var file = 
    fs.readFileSync(path.join(__dirname, "./public/export.html"))
    .toString()
    .replace(exportsRegex("STARS"), stars)
    .replace(exportsRegex("D3"), d3)
    .replace(exportsRegex("GEO"), geo)
    .replace(exportsRegex("CSS"), css)
  function exportsRegex(str) { return new RegExp(`(\\[\\{${str}\\}\\])`, "g") }
  res.send(file)
})

app.post('/predictions/partition', (req, res) => {
  if (!req.body || !(req.body instanceof Array) || !req.body.length) {
    res.sendStatus(400);
    return
  }
  const predictionsArray = req.body[0] instanceof Array ? req.body : [req.body]

  var newPredictions = predictionsToStars(predictionsArray)
  var savedPredictions = ""

  if (fs.existsSync(tempFile)) {
    savedPredictions = fs.readFileSync(tempFile, { encoding: "utf8" }).toString()
    if (savedPredictions.trim()) {
      savedPredictions += ","
    }
  }

  const newData = savedPredictions + newPredictions
  fs.writeFileSync(tempFile, newData, { encoding: "utf8" });

  res.sendStatus(200);
})

app.post('/predictions/save', (req, res) => {
  if (!fs.existsSync(tempFile)) {
    res.statusCode = 404;
    res.send("Please send data to http://localhost:3000/predictions/partition first")
    return
  }
  var savedPredictions = fs.readFileSync(tempFile, { encoding: "utf8" }).toString()
  const data = `{ "type": "FeatureCollection", "features": [${savedPredictions}] }`

  fs.writeFileSync(tempFile, "");
  fs.writeFileSync(path.join(__dirname, "./public/data/stars.json"), data)
  res.sendStatus(200);
})

app.delete('/predictions/clear', (req, res) => {
  var data = fs.readFileSync(path.join(__dirname, "./public/data/stars.def.json"), { encoding: "utf8" }).toString()
  fs.writeFileSync(tempFile, "");
  fs.writeFileSync(path.join(__dirname, "./public/data/stars.json"), data)
  res.sendStatus(200)
})

app.use(express.static('public'))

app.listen(3000, () => console.log("Listening on http://localhost:3000"))

function predictionsToStars(predictions) {
  const xRange = 358
  const yRange = 280
  const lastIndex = predictions.length - 1
  return predictions.map((v, index) => {
    const goodKepler = v[1] > 0.8
    const x = Math.random() * xRange - 179
    const y = Math.random() * yRange - 179
    return `{
      "type": "Feature",
      "id": ${v[0]},
      "properties": {
        "name": "${goodKepler ? "Kepler " + v[0] : ""}",
        "desig": "${goodKepler ? v[1] : ""}",
        "con": "",
        "mag": -1,
        "bv": ${v[1] * 4}
      },
      "geometry": {
        "type": "Point",
        "coordinates": [${x}, ${y}]
      }
    } ${index === lastIndex ? '' : ','}`
  }).join("")
}

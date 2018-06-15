# Stars viewer

Node.js server show the stars in the space.

## Quick start

Clone repository
```
git clone /repos link/
```

Install packages
```
npm i
```

Start node.js server

```
npm start
```

Open in browser http://localhost:3000/

## Upload new predictions

### Send predictions to server

Make a post request to http://localhost:3000/predictions/partition
With a body like:

```
[
  [ kepler_id, prediction ]
]
```

Where
  kepler_id is Kepler identity (Number)
  prediction is prediction for this star (Number in range [0, 1])

### Save predictions

Make a post request to http://localhost:3000/predictions/save without body


## Clear predictions

Remove your predictions from dataset and uploads default predictions

Make a delete request to http://localhost:3000/predictions/clear

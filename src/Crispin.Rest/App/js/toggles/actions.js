import "babel-polyfill";
import makeAsync from "../util/async-action";

const allToggles = makeAsync("ALL_TOGGLES", () =>
  fetch(`/api/toggles`)
    .then(response => response.json())
    .then(toggles => ({ toggles }))
);

const updateToggleName = makeAsync("UPDATE_TOGGLE_NAME", ({ toggleID, name }) =>
  fetch(`/api/toggles/id/${toggleID}/name`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ name: name })
  }).then(response => response.json())
);

const desc = makeAsync(
  "UPDATE_TOGGLE_DESCRIPTION",
  ({ toggleID, description }) =>
    fetch(`/api/toggles/id/${toggleID}/description`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ description: description })
    }).then(response => response.json())
);

const create = makeAsync("CREATE_TOGGLE", ({ name, description }) =>
  fetch(`/api/toggles`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ name, description })
  }).then(response => response.json())
);

export const FETCH_ALL_TOGGLES_STARTED = allToggles.started;
export const FETCH_ALL_TOGGLES_FINISHED = allToggles.finished;
export const fetchAllToggles = allToggles.action;

export const UPDATE_TOGGLE_NAME_STARTED = updateToggleName.started;
export const UPDATE_TOGGLE_NAME_FINISHED = updateToggleName.finished;
export const updateName = updateToggleName.action;

export const UPDATE_TOGGLE_DESCRIPTION_STARTED = desc.started;
export const UPDATE_TOGGLE_DESCRIPTION_FINISHED = desc.finished;
export const updateDescription = desc.action;

export const CREATE_TOGGLE_STARTED = create.started;
export const CREATE_TOGGLE_FINISHED = create.finished;
export const createToggle = create.action;

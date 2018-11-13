import "babel-polyfill";

export const ALL_TOGGLES_REQUESTED = "ALL_TOGGLES_REQUESTED";
export const ALL_TOGGLES_RECEIVED = "ALL_TOGGLES_RECEIVED";
export const UPDATE_TOGGLE_NAME_REQUESTED = "UPDATE_TOGGLE_NAME_REQUESTED";
export const UPDATE_TOGGLE_NAME_COMPLETED = "UPDATE_TOGGLE_NAME_COMPLETED";
export const UPDATE_TOGGLE_DESCRIPTION_REQUESTED =
  "UPDATE_TOGGLE_DESCRIPTION_REQUESTED";
export const UPDATE_TOGGLE_DESCRIPTION_COMPLETED =
  "UPDATE_TOGGLE_DESCRIPTION_COMPLETED";

const allTogglesRequested = () => ({
  type: ALL_TOGGLES_REQUESTED
});

const allTogglesReceived = toggles => ({
  type: ALL_TOGGLES_RECEIVED,
  toggles
});

export const fetchAllToggles = () => async dispatch => {
  dispatch(allTogglesRequested());

  const response = await fetch(`/api/toggles`);
  const json = await response.json();
  dispatch(allTogglesReceived(json));
};

const updateToggleNameRequested = (toggleID, newName) => ({
  type: UPDATE_TOGGLE_NAME_REQUESTED,
  toggleID,
  newName
});

const toggleNameUpdated = (toggleID, newName) => ({
  type: UPDATE_TOGGLE_NAME_COMPLETED,
  toggleID,
  newName
});

export const updateName = (toggleID, newName) => async dispatch => {
  dispatch(updateToggleNameRequested(toggleID, newName));

  const response = await fetch(`/api/toggles/id/${toggleID}/name`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ name: newName })
  });

  const json = await response.json();

  dispatch(toggleNameUpdated(json.toggleID, json.name));
};

const updateToggleDescriptionRequested = (toggleID, newDescription) => ({
  type: UPDATE_TOGGLE_DESCRIPTION_REQUESTED,
  toggleID,
  newDescription
});

const toggleDescriptionUpdated = (toggleID, newDescription) => ({
  type: UPDATE_TOGGLE_DESCRIPTION_COMPLETED,
  toggleID,
  newDescription
});

export const updateDescription = (
  toggleID,
  newDescription
) => async dispatch => {
  dispatch(updateToggleDescriptionRequested(toggleID, newDescription));

  const response = await fetch(`/api/toggles/id/${toggleID}/description`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify({ description: newDescription })
  });

  const json = await response.json();

  dispatch(toggleDescriptionUpdated(json.toggleID, json.description));
};

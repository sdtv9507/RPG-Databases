extends Container


# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var iconPath = ""
var state_selected = 0
var add_string = -1
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start():
	var json_dictionary = get_parent().get_parent().call("read_data", "State")
	$StateButton.clear()
	for i in range(json_dictionary.size()):
		var stateData = json_dictionary["state" + String(i)]
		if i > $StateButton.get_item_count() - 1:
			$StateButton.add_item(stateData["name"])
		else:
			$StateButton.set_item_text(i, stateData["name"])
	refresh_data(state_selected)

func refresh_data(id):
	var json_dictionary = get_parent().get_parent().call("read_data", "State")
	var stateData = json_dictionary["state" + id]
	var eraseConditions = stateData["erase_conditions"]
	var messages = stateData["messages"]
	var customEraseConditions = stateData["custom_erase_conditions"]
	$NameLabel/NameLine.text = stateData["name"]
	var icon = stateData["icon"]
	if icon != "":
		iconPath = String(stateData["icon"])
		$IconLabel/Sprite.texture = load(stateData["icon"])
	$RestrictionLabel/RestrictionOption.selected = int(stateData["restriction"])
	$PriorityLabel/Priorityvalue.value = int(stateData["priority"])
	$EraseLabel/TurnsLabel/MinTurns.value = int(eraseConditions["turns_min"])
	$EraseLabel/TurnsLabel/MaxTurns.value = int(eraseConditions["turns_max"])
	$EraseLabel/DamageLabel/Damage.value = int(eraseConditions["erase_damage"])
	$EraseLabel/SetpsLabel/SpinBox.value = int(eraseConditions["erase_setps"])
	$MessagesLabel/PanelContainer/VBoxContainer/MessageList.clear()
	for message in range(messages.keys()):
		$MessagesLabel/PanelContainer/VBoxContainer/MessageList.add_item(message)
	$CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.clear()
	for condition in range(customEraseConditions.keys()):
		$CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.add_item(condition)
	if (stateData.has("effects") == true):
		clear_effect_list()
		var effectList = stateData["effects"]
		for effect in effectList:
			add_effect_list(effect["name"], effect["data_id"], effect["value1"], effect["value2"])

func _on_SearchState_pressed():
	$IconLabel/SearchDialog.popup_centered()

func _on_SearchStateIconDialog_file_selected(path):
	iconPath = path
	$IconLabel/Sprite.texture = load(path)

func _on_AddCustomStateCondition_pressed():
	add_string = 0
	$AddString.WindowTitle = "Custom State Erase Formula"
	$AddString.popup_centered()

func _on_RemoveCustomStateCondition_pressed():
	var selected = $CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.GetselectedItems()[0]
	if (selected > -1):
		$CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.remove_item(selected)

func _on_AddStateMessage_pressed():
	add_string = 1
	$AddString.WindowTitle = "State Message"
	$AddString.popup_centered()

func _on_RemoveStateMessage_pressed():
	var selected = $MessagesLabel/PanelContainer/VBoxContainer/MessageList.GetselectedItems()[0]
	if (selected > -1):
		$MessagesLabel/PanelContainer/VBoxContainer/MessageList.remove_item(selected)

func _on_ConfirmAddString_pressed():
	var text = $AddString/LineEdit.text
	if (add_string == 0):
		$CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.add_item(text)
	elif (add_string == 1):
		$MessagesLabel/PanelContainer/VBoxContainer/MessageList.add_item(text)
	add_string = -1
	$AddString.hide()

func _on_CancelAddString_pressed():
	add_string = -1
	$AddString.hide()

func _on_StateButton_item_selected(id):
	state_selected = id
	refresh_data(id)

func _on_AddState_pressed():
	$StateButton.add_item("NewState")
	var id = $StateButton.get_item_count() - 1
	var json_dictionary = get_parent().get_parent().call("read_data", "State")
	var stateData
	var eraseCondition
	var messages
	var customEraseConditions
	stateData["name"] = "NewState"
	stateData["icon"] = ""
	stateData["restriction"] = 4
	stateData["priority"] = 100
	eraseCondition["turns_min"] = 0
	eraseCondition["turns_max"] = 0
	eraseCondition["erase_damage"] = 0
	eraseCondition["erase_setps"] = 0
	stateData["erase_conditions"] = eraseCondition
	messages["0"] = "Insert a custom message"
	stateData["messages"] = messages
	customEraseConditions["0"] = "Insert a custom condition"
	stateData["custom_erase_conditions"] = customEraseConditions
	json_dictionary["state" + id] = stateData
	get_parent().get_parent().call("store_data", "State", json_dictionary)

func _on_RemoveState_pressed():
	var json_dictionary = get_parent().get_parent().call("read_data", "State")
	if (json_dictionary.keys().size() > 1):
		var stateId = state_selected
		while (stateId < json_dictionary.Keys.size() - 1):
			json_dictionary["state" + stateId] = json_dictionary["state" + (stateId + 1)]
			stateId += 1
		json_dictionary.erase("state" + String(stateId))
		get_parent().get_parent().call("store_data", "State", json_dictionary)
		$StateButton.remove_item(state_selected)
		if (state_selected == 0):
			$StateButton.select(state_selected + 1)
			state_selected += 1
		else:
			$StateButton.select(state_selected - 1)
			state_selected -= 1
		$StateButton.select(state_selected)
		refresh_data(state_selected)

func _on_SaveStates_pressed():
	var json_dictionary = get_parent().get_parent().call("read_data", "State")
	var stateData = json_dictionary["state" + String(state_selected)]
	var eraseCondition = stateData["erase_conditions"]
	var messages = stateData["messages"]
	var customEraseConditions = stateData["custom_erase_conditions"]
	var effectList

	stateData["name"] = $NameLabel/NameLine.text
	stateData["icon"] = iconPath
	stateData["restriction"] = $RestrictionLabel/RestrictionOption.selected
	stateData["priority"] = $PriorityLabel/Priorityvalue.value
	eraseCondition["turns_min"] = $EraseLabel/TurnsLabel/MinTurns.value
	eraseCondition["turns_max"] = $EraseLabel/TurnsLabel/MaxTurns.value
	eraseCondition["erase_damage"] = $EraseLabel/DamageLabel/Damage.value
	eraseCondition["erase_setps"] = $EraseLabel/SetpsLabel/SpinBox.value
	var items = $MessagesLabel/PanelContainer/VBoxContainer/MessageList.get_item_count()
	for i in range(items):
		messages[String(i)] = $MessagesLabel/PanelContainer/VBoxContainer/MessageList.get_item_text(i)
	items = $CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.get_item_count()
	for i in range(items):
		customEraseConditions[String(i)] = $CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions.get_item_text(i)
	var effectSize = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_count()
	for i in range(effectSize):
		var effectData
		effectData["name"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_text(i)
		effectData["data_id"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.get_item_text(i)
		effectData["value1"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/Effectvalue1.get_item_text(i)
		effectData["value2"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/Effectvalue2.get_item_text(i)
		effectList[effectData]
	stateData["effects"] = effectList
	get_parent().get_parent().call("store_data", "State", json_dictionary)
	refresh_data(state_selected)

func _on_AddStateEffect_pressed() -> void:
	get_parent().get_parent().call("open_effect_manager", "Character")

func _on_RemoveStateEffect_pressed() -> void:
	var id = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_selected_items()[0]
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/Effectvalue1.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/Effectvalue2.remove_item(id)

func add_effect_list(name: String, data_id: int, value1: String, value2: String) -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.add_item(name)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.add_item(data_id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/Effectvalue1.add_item(value1)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/Effectvalue2.add_item(value2)

func clear_effect_list() -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/Effectvalue1.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/Effectvalue2.clear()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
